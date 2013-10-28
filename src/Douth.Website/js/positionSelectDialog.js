(function($){
    $.widget("ui.positionSelectDialog", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._getTopPositionUrl = $.baseUrl + "Org/TopPosition";
                this._getPositionsUrl = $.baseUrl + "Org/Positions";
                this._modal = this.element.find(".modal");
                var positionTree = this.element.find(".positionTree");
                this._positionTree = positionTree;
                $.get(this._getTopPositionUrl, null, function(model){
                    if(model.result == 0){
                        positionTree.tree({
			                data: [
				                model.data
			                ],
			                dblclickOpen: true,
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
                this.element.find(".btnOk").click(function(){
                    var node = positionTree.tree("getSelectedNode");
                    var positionId = node.treenode("option", "id");
                    thiz._modal.modal("hide");
                    thiz._createdCallback(positionId);
                    return false;
                });
                
	        },
            select: function(callback){
                this._createdCallback = callback;
                this._modal.modal("show");
                var topNode = this._positionTree.tree("getTopNode");
                topNode.treenode("reload");
            }
        }
    );
})(jQuery);