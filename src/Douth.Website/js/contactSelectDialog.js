(function($){
    $.widget("ui.contactSelectDialog", {
            options: {
                baseUrl: null,
                singleSelect: true
	        },
	        _create: function(){
                var thiz = this;
                this._dataGetUrl = this.options.baseUrl + "Contact/SelectCustomers";
                this._modal = this.element.find(".modal");
                this._btnSelect = this.element.find(".btnSelect").click(function(){
                    var row = thiz._contactGrid.datagrid("getSelectedRow");
                    var id = row.datarow("option", "data").id;
                    var name = row.datarow("option", "data").name;
                    var result = thiz._selectCallback({id: id, name: name});
                    if(result == undefined || result == true){
                        thiz._modal.modal("hide");
                    }
                });
                $(".btnKeywordSearch").click(function(){
                    var btn = $(this).button("loading");
                    thiz.loadContactGrid(0, function(){
                        btn.button("reset");
                    });
                    return false;
                });
                this._txtKeyword = this.element.find(".txtKeyword");
                this._contactGrid = this.element.find(".contactGrid").datagrid({
		            columns: [{title: "姓名", width: 100, field: "name"},{title: "客户名称", width: 300, field: "customer"}],
                    height: 280,
		            canSort: false,
		            singleSelect: thiz.options.singleSelect,
		            showNumberRow: true,
                    selectedRow: function(){
                        thiz._btnSelect.prop("disabled", false);
                    },
                    unselectedRow: function(){
                        var rows = thiz._contactGrid.datagrid("getSelectedRows");
                        thiz._btnSelect.prop("disabled", rows.length == 0);
                    }
	            });
                this._contactPager = this.element.find(".contactPager")
                  .pager({change: function(event, args){
                      thiz.loadContactGrid(args.start);
                  }});
	        },
            select: function(callback){
                this._selectCallback = callback;
                var pageInfo = this._contactPager.pager("option").pageInfo;
                var start = pageInfo.start || 0;
                this.loadContactGrid(start);
                this._modal.modal("show");
            },
            loadContactGrid: function(start, cb){
                var thiz = this;
                var args = {start: start, size: 20, keyword: this._txtKeyword.val()};
                $.get(this._dataGetUrl, args, function(model){
                    if(cb){
                        cb();
                    }
                    if(model.result == 0){
                        thiz._contactGrid.datagrid("option", "data", model.data.list);
                        thiz._contactPager.pager("option", "pageInfo", {start: start, size: 20, count: model.data.count})
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