(function($){
    $.widget("ui.metadataSelectDialog", {
            options: {
                singleSelect: true
	        },
	        _create: function(){
                var thiz = this;
                this._dataGetUrl = $.baseUrl + "Metadata/SelectMetadatas";
                this._modal = this.element.find(".modal");
                this._headerName = this.element.find(".header-name");
                this._btnSelect = this.element.find(".btnSelect").click(function(){
                    var row = thiz._metadataGrid.datagrid("getSelectedRow");
                    var id = row.datarow("option", "data").id;
                    var name = row.datarow("option", "data").name;
                    var result = thiz._selectCallback({id: id, name: name});
                    if(result == undefined || result == true){
                        thiz._modal.modal("hide");
                    }
                });
                $(".btnKeywordSearch").click(function(){
                    var btn = $(this).button("loading");
                    thiz.loadMetadataGrid(0, function(){
                        btn.button("reset");
                    });
                    return false;
                });
                this._txtKeyword = this.element.find(".txtKeyword");
                this._metadataGrid = this.element.find(".metadataGrid").datagrid({
		            columns: [{title: "名称", width: 350, field: "name"}],
                    height: 280,
		            canSort: false,
		            singleSelect: thiz.options.singleSelect,
		            showNumberRow: true,
                    selectedRow: function(){
                        thiz._btnSelect.prop("disabled", false);
                    },
                    unselectedRow: function(){
                        var rows = thiz._metadataGrid.datagrid("getSelectedRows");
                        thiz._btnSelect.prop("disabled", rows.length == 0);
                    }
	            });
                this._metadataPager = this.element.find(".metadataPager")
                  .pager({change: function(event, args){
                      thiz.loadMetadataGrid(args.start);
                  }});
	        },
            select: function(formId, formName, callback){
                this._headerName.text(formName);
                this._selectCallback = callback;
                this._formId = formId;
                var pageInfo = this._metadataPager.pager("option").pageInfo;
                var start = pageInfo.start || 0;
                this.loadMetadataGrid(start);
                this._modal.modal("show");
            },
            loadMetadataGrid: function(start, cb){
                var thiz = this;
                var args = {formId: this._formId, start: start, size: 20, keyword: this._txtKeyword.val()};
                $.get(this._dataGetUrl, args, function(model){
                    if(cb){
                        cb();
                    }
                    if(model.result == 0){
                        thiz._metadataGrid.datagrid("option", "data", model.data.list);
                        thiz._metadataPager.pager("option", "pageInfo", {start: start, size: 20, count: model.data.count})
                        thiz._btnSelect.prop("disabled", true);
                    }
                    else{
                        alert(model.message)
                    }
                });
            }
        }
    );
})(jQuery);
$(document).ready(function() {
    var metadataSelectDialog = $(".metadataSelectDialog").metadataSelectDialog();
    $(".metadataSelect").metadataSelect({metadataSelectDialog: metadataSelectDialog});
});