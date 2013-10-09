(function($){
    $.widget("ui.customerSelectDialog", {
            options: {
                baseUrl: null,
                singleSelect: true
	        },
	        _create: function(){
                var thiz = this;
                this._dataGetUrl = this.options.baseUrl + "Customer/SelectCustomers";
                this._modal = this.element.find(".modal");
                this._btnSelect = this.element.find(".btnSelect").click(function(){
                    var row = thiz._customerGrid.datagrid("getSelectedRow");
                    var id = row.datarow("option", "data").id;
                    var name = row.datarow("option", "data").name;
                    var result = thiz._selectCallback({id: id, name: name});
                    if(result == undefined || result == true){
                        thiz._modal.modal("hide");
                    }
                });
                $(".btnKeywordSearch").click(function(){
                    var btn = $(this).button("loading");
                    thiz.loadCustomerGrid(0, function(){
                        btn.button("reset");
                    });
                    return false;
                });
                this._txtKeyword = this.element.find(".txtKeyword");
                this._customerGrid = this.element.find(".customerGrid").datagrid({
		            columns: [{title: "名称", width: 350, field: "name"}],
                    height: 280,
		            canSort: false,
		            singleSelect: thiz.options.singleSelect,
		            showNumberRow: true,
                    selectedRow: function(){
                        thiz._btnSelect.prop("disabled", false);
                    },
                    unselectedRow: function(){
                        var rows = thiz._customerGrid.datagrid("getSelectedRows");
                        thiz._btnSelect.prop("disabled", rows.length == 0);
                    }
	            });
                this._customerPager = this.element.find(".customerPager")
                  .pager({change: function(event, args){
                      thiz.loadCustomerGrid(args.start);
                  }});
	        },
            select: function(callback){
                this._selectCallback = callback;
                var pageInfo = this._customerPager.pager("option").pageInfo;
                var start = pageInfo.start || 0;
                this.loadCustomerGrid(start);
                this._modal.modal("show");
            },
            loadCustomerGrid: function(start, cb){
                var thiz = this;
                var args = {start: start, size: 20, keyword: this._txtKeyword.val()};
                $.get(this._dataGetUrl, args, function(model){
                    if(cb){
                        cb();
                    }
                    if(model.result == 0){
                        thiz._customerGrid.datagrid("option", "data", model.data.list);
                        thiz._customerPager.pager("option", "pageInfo", {start: start, size: 20, count: model.data.count})
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