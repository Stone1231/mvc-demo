function RWData(queryDiv, listDiv, editDiv) {
    this.queryDiv = $("#" + queryDiv);
    this.queryForm = this.queryDiv.find("form:first");
    this.listDiv = $("#" + listDiv);
    this.editDiv = $("#" + editDiv);

    this.pageId = "page";
    this.currentPage = $("#" + this.pageId);
    if (!this.currentPage.length) {
        this.currentPage = $('<input>').attr({ type: 'hidden', id: this.pageId, name: this.pageId, value: 1 });
       
        //$('<input>')
        //    .attr({ type: 'hidden', id: this.pageId, name: this.pageId, value: 1 })
        //    .appendTo(this.queryForm);
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
        editForm.removeData('validator');
        editForm.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(editForm);
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

        var formElement = editForm.get(0);
        var formURL = formElement.action;
        var oData = new FormData(formElement);
        var oReq = new XMLHttpRequest();
        var rwData = this;
        oReq.open("POST", formURL, true);
        oReq.onload = function (oEvent) {
            if (oReq.status == 200) {
                var jsonObj = JSON.parse(oReq.response);
                if (jsonObj.success) {
                    rwData.closeEdit();
                    rwData.queryData();
                }
                else {
                    //if (jsonObj.data) {
                    //    $('span[data-valmsg-for]').hide();
                    //    for (var i in jsonObj.data) {
                    //        var item = jsonObj.data[i];
                    //        $('span[data-valmsg-for="' + item.key + '"]').html(item.errors.join("、"));
                    //        $('span[data-valmsg-for="' + item.key + '"]').show();
                    //    }
                    //}
                    //else {
                    //    alert("error");
                    //}
                }

                if (jsonObj.message != "") {
                    alert(jsonObj.message);
                }

            } else {
                alert("Error " + oReq.status + " occurred uploading your file.<br \/>");
            }
        };
        oReq.send(oData);
    }

    this.queryData = function () {

        //避免不只一個列表
        this.currentPage.appendTo(this.queryForm);
        this.sortCol.appendTo(this.queryForm);
        this.sortDesc.appendTo(this.queryForm);

        var formElement = this.queryForm.get(0);
        var formURL = formElement.action;
        var oData = new FormData(formElement);
        var oReq = new XMLHttpRequest();
        var rwData = this;
        oReq.open("POST", formURL, true);
        oReq.onload = function (oEvent) {
            if (oReq.status == 200) {
                rwData.listDiv.html(oReq.response);
                rwData.setPage();

                rwData.resetUI();
            } else {
                alert("Error " + oReq.status + " occurred uploading your file.<br \/>");
            }
        };
        oReq.send(oData);
    }

    this.resetUI;
}









