(function($){
    $.widget("ui.positionManager", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._getTopPositionUrl = $.baseUrl + "Org/TopPosition";
                this._getPositionsUrl = $.baseUrl + "Org/Positions";
                
                var btnCreatePosition = this.element.find(".btnCreatePosition");
                var btnEditPositoin = this.element.find(".btnEditPositoin");
                var btnDeletePosition = this.element.find(".btnDeletePosition");
                var positionCreateDialog = this.element.find(".positionCreateDialog").positionCreateDialog();
                var positoinEditDialog = this.element.find(".positoinEditDialog").positoinEditDialog();
                var positionTree = this.element.find(".positionTree");
                
                btnCreatePosition.click(function(){
                    var node = positionTree.tree("getSelectedNode");
                    var positionId = node.treenode("option", "id");
                    positionCreateDialog.positionCreateDialog("create", positionId, function(position){
                        node.treenode("reload").treenode("expand");
                    });
                    return false;
                });
                btnEditPositoin.click(function(){
                    var node = positionTree.tree("getSelectedNode");
                    var id = node.treenode("option", "id");
                    var name = node.treenode("option", "text");
                    positoinEditDialog.positoinEditDialog("edit", {id: id, name: name}, function(position){
                        node.treenode("option", "text", position.text);
                    });
                    return false;
                });
                btnDeletePosition.click(function(){
                    if(!confirm("确实要删除吗?"))
                    {
                        return false;
                    }
                    var node = positionTree.tree("getSelectedNode");
                    var positionId = node.treenode("option", "id");
                    $.post($.baseUrl + "Org/DeletePosition", {positionId: positionId}, function(model){
                        if(model.result == 0){
                            node.treenode("remove");
                            positionTree.tree("selectTopNode");
                        }
                        else{
                            alert(model.message)
                        }
                    });
                    return false;
                });
                $.get(this._getTopPositionUrl, null, function(model){
                    if(model.result == 0){
                        positionTree.tree({
			                data: [
				                model.data
			                ],
			                dblclickOpen: true,
                            treenodeSelected: function(sender, node){
                                thiz._trigger("treenodeSelected", null, node);
                            },
			                treenodeLoading: function(event, treenode){
                            var positionId = treenode.treenode("option", "id");
                                $.get(thiz._getPositionsUrl, {parentId: positionId}, function(model){
                                    if(model.result == 0){
                                        $.each(model.data, function(i, position){
				                            treenode.treenode("append", position);
                                        })
                                    }
                                    else{
                                        alert(model.message)
                                    }
                                });
			                }
		                })
                        .tree("expandTopNode")
                        .tree("selectTopNode");
                    }
                    else{
                        alert(model.message)
                    }
                });
                
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