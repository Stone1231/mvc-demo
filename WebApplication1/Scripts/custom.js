function setTextAeraReset() {
    $(function () {
        var contents = [];

        $("textarea").each(function (e) {
            var textArea = $(this);
            var edit = textArea.parent().find(".note-editable").first();
            contents[e] = edit.html();
        });
        $(":input[type='reset']").click(function () {
            $("textarea").each(function (e) {
                var textArea = $(this);
                var edit = textArea.parent().find(".note-editable").first();
                edit.html(contents[e]);
            });
        })
    })
}

function setBtnImg(div) {
    var _div = "";
    if (div != null) {
        _div = "#" + div + " ";
    }

    $("<i class=\"fa fa-check\"></i>").prependTo(_div + ".btn-success");
    $("<i class=\"fa fa-paste\"></i>").prependTo(_div + ".btn-primary");
    $("<i class=\"fa fa-times\"></i>").prependTo(_div + ".btn-danger");
}

var isdelete = false;
function confirmDel(link) {
    if (isdelete) {
        isdelete = false;
        return true;
    }

    swal({
        title: "確認刪除",
        text: "點選確認後將會刪除此資料",
        type: "warning",
        confirmButtonText: "確認",
        cancelButtonText: "取消",
        showCancelButton: true
    },
    function (isConfirm) {
        isdelete = isConfirm;
        if (isdelete) {
            $(link).click();
        }
    }
    );
    return false;
}

function showSuccess() {
    //swal({
    //    title: "存檔成功",
    //    text: "存檔資料已表列如下",
    //    confirmButtonText: "確認",
    //    type: "success"
    //});

    alert("存檔成功");
}

function showError(msg) {
    //swal({
    //    title: "失敗",
    //    text: msg,
    //    confirmButtonText: "確認",
    //    type: "warning"
    //});

    alert(msg);
}

function setPlaceholder() {
    $(":input").each(function () {
        var input = $(this);
        var attr = input.attr("title");

        if (typeof attr !== typeof undefined && attr !== false) {
            input.attr("placeholder", input.attr("title"));
        }
    });
}

function setValidation(form) {
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
}

function SendData(formElement) {
    var formURL = formElement.action;
    var oData = new FormData(formElement);
    var oReq = new XMLHttpRequest();

    var sendData = this;

    oReq.open("POST", formURL, true);
    oReq.onload = function (oEvent) {
        if (oReq.status == 200) {
            sendData.success(oReq);
        } else {
            sendData.error(oReq);
            //alert("Error " + oReq.status + " occurred uploading your file.<br \/>");
        }
    };
    oReq.send(oData);

    this.success;

    this.error;
}

function RWData(queryDiv, listDiv, editDiv) {
    this.queryDiv = $("#" + queryDiv);
    this.queryForm = this.queryDiv.find("form:first");
    this.listDiv = $("#" + listDiv);
    this.editDiv = $("#" + editDiv);

    this.pageId = "page";
    this.currentPage = $("#" + this.pageId);
    if (!this.currentPage.length) {
        this.currentPage = $('<input>').attr({ type: 'hidden', id: this.pageId, name: this.pageId, value: 1 });
    }

    this.sortColId = "sortCol";
    this.sortCol = $("#" + this.sortColId);
    if (!this.sortCol.length) {
        this.sortCol = $('<input>').attr({ type: 'hidden', id: this.sortColId, name: this.sortColId });
    }

    this.sortDescId = "sortDesc";
    this.sortDesc = $("#" + this.sortDescId);
    if (!this.sortDesc.length) {
        this.sortDesc = $('<input>').attr({ type: 'hidden', id: this.sortDescId, name: this.sortDescId, value: "false" });
    }

    /*搭配MvcPaging*/
    this.setPage = function () {
        var rwData = this;
        $('.pager> a').each(function (i, item) {
            var url = $(item).attr('href');
            var n = url.indexOf('page=');
            var page = url.substring(n).replace('page=', '');
            $(item).attr('href', '#').click(function () { rwData.postPage(page); });
        });
    }

    this.postPage = function (page) {
        if (this.queryForm.size() > 0) {
            this.currentPage.val(page);
            this.queryData();
        }
    }

    this.postSort = function (sort) {
        if (this.queryForm.size() > 0) {
            if (this.sortCol.val() == sort) {
                var desc = this.sortDesc.val() === "true" ? "false" : "true";
                this.sortDesc.val(desc);
            }
            else {
                this.sortDesc.val("false");
            }
            this.sortCol.val(sort);
            this.bindData();
        }
    }

    this.openEdit = function () {
        this.editDiv.show();
        this.listDiv.hide();
        this.queryDiv.hide();

        //重設驗證
        var editForm = this.editDiv.find("form:first");
        setValidation(editForm);
    }

    this.closeEdit = function () {
        this.editDiv.html("");
        this.listDiv.show();
        this.queryDiv.show();
    }

    this.bindData = function () {
        if (this.currentPage.length) {
            this.currentPage.val(1);
        }
        this.queryData();
    }

    this.saveData = function () {
        var editForm = this.editDiv.find("form:first");
        if (!editForm.valid()) {
            return;
        }

        this.saveBefore();

        var rwData = this;

        var formElement = editForm.get(0);
        var sendData = new SendData(formElement);

        sendData.success = function (oReq) {
            var jsonObj = JSON.parse(oReq.response);
            if (jsonObj.success) {
                rwData.closeEdit();
                rwData.queryData();
                //msg
                showSuccess();
            }
            else {              
                showError(jsonObj.message);
            }
        }

        sendData.error = function (oReq) {
            showError(oReq.response);
        }
    }

    this.queryData = function () {

        //避免不只一個列表
        this.currentPage.appendTo(this.queryForm);
        this.sortCol.appendTo(this.queryForm);
        this.sortDesc.appendTo(this.queryForm);

        var rwData = this;

        var formElement = this.queryForm.get(0);
        var sendData = new SendData(formElement);

        sendData.success = function (oReq) {
            rwData.listDiv.html(oReq.response);
            rwData.setPage();
            rwData.queryEnd();
        }

        sendData.error = function (oReq) {
            //msg
            //alert("Error " + oReq.status + " " + oReq.response);
            showError("Error " + oReq.status + " " + oReq.response);
        }
    }

    /*搭配footable 在server取資料*/
    this.setFootable = function (pageNumber, pageCount) {
        var rwData = this;

        var table = $(".footable").first();

        //sort
        var tr = table.find("thead").first().find("tr");

        tr.find("th[data-sort-ignore!='true']").addClass("footable-sortable").append("<span class=\"footable-sort-indicator\"></span>");

        if (rwData.sortCol.val() != '') {
            var inputCol = tr.find(":input[value='" + rwData.sortCol.val() + "']");
            var th = inputCol.parent();

            if (rwData.sortDesc.val() == "true") {
                th.addClass("footable-sortable footable-sorted-desc");
            }
            else {
                th.addClass("footable-sortable footable-sorted");
            }
        }

        tr.find("th").click(function () {
            rwData.postSort($(this).find(":hidden").val());
        })

        //page
        var trLast = table.find("tfoot").first().find("tr");
        var ul = trLast.find("ul").first();

        if (pageCount <= 1) {
            ul.hide();
            return;
        }

        ul.html("");

        var arrowClass = "footable-page-arrow";
        var disarrowClass = "footable-page-arrow disabled";
        var pageClass = "footable-page";
        var activepageClass = "footable-page active";

        var liFirst = $("<li>");
        var liPrev = $("<li>");
        var liNext = $("<li>");
        var liLast = $("<li>");

        var hrefStr = "javascript:void(0);";//'#'
        var aFirst = $("<a>").attr({ href: hrefStr }).html("«"); aFirst.appendTo(liFirst);
        var aPrev = $("<a>").attr({ href: hrefStr }).html("‹"); aPrev.appendTo(liPrev);
        var aNext = $("<a>").attr({ href: hrefStr }).html("›"); aNext.appendTo(liNext);
        var aLast = $("<a>").attr({ href: hrefStr }).html("»"); aLast.appendTo(liLast);

        if (pageNumber == 1) {
            liFirst.addClass(disarrowClass);
            liPrev.addClass(disarrowClass);
        }
        else {
            liFirst.addClass(arrowClass);
            liPrev.addClass(arrowClass);
            aFirst.click(function () {
                rwData.postPage(1);
            })
            aPrev.click(function () {
                rwData.postPage(pageNumber - 1);
            })
        }

        if (pageNumber == pageCount) {
            liNext.addClass(disarrowClass);
            liLast.addClass(disarrowClass);
        }
        else {
            liNext.addClass(arrowClass);
            liLast.addClass(arrowClass);
            aNext.click(function () {
                rwData.postPage(pageNumber + 1);
            })
            aLast.click(function () {
                rwData.postPage(pageCount);
            })
        }

        liFirst.appendTo(ul);
        liPrev.appendTo(ul);

        for (var i = 1; i <= pageCount; i++) {
            var li = $("<li>");
            var a = $("<a>").attr({ href: hrefStr }).html(i); a.appendTo(li);
            if (pageNumber == i) {
                li.addClass(activepageClass);
            }
            else {
                li.addClass(pageClass);
                a.click(function () {
                    var p = $(this).html();
                    rwData.postPage(p);
                })
            }
            li.appendTo(ul);
        }

        liNext.appendTo(ul);
        liLast.appendTo(ul);
    }

    this.queryEnd = function () { };

    this.saveBefore = function () { };
}