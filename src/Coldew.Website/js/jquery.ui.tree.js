(function( $, undefined ) {
$.widget( "ui.treeBase", {
	_selectedNodes: null,
	options: {
		data: null,
		dblclickOpen: false
	},
	_create: function() {
		var self = this;
		this._selectedNodes = [];
		self.element.addClass("ui-tree");
		
	},
	_init: function(){
		var self = this;
		if(this.options.data && this.options.data.length){
			$.each(this.options.data, function(i, node){
				var treenode = $("<div></div>");
				self.element.append(treenode);
                node.showCheckbox = self._isShowCheckbox();
				treenode.treenode(node);
		
				treenode.bind("treenodeloading", function(event, node){self._onNodeLoading($(node));})
				.bind("treenodeclick", function(event, node){self._onNodeClick($(node));})
				.bind("treenodedblclick", function(event, node){self._onNodeTreenodeDblclick($(node));})
				.bind("treenodeselected", function(event, node){self._onNodeSelected($(node));})
				.bind("treenodeunselected", function(event, node){self._onNodeUnselected($(node));})
				.bind("treenodeadded", function(event, node){self._onNodeAdded($(node));})
				.bind("treenoderemoved", function(event, node){self._onNodeRemoved($(node));});
			});
		}
	},
	_isShowCheckbox: function(){
		return false;
	},
	_onNodeAdded: function(node){
		this._trigger("treenodeAdded", null, node);
	},
	_onNodeRemoved: function(node){
		this._trigger("treenodeRemoved", null, node);
	},
	_onNodeSelected: function(node){
		this._trigger("treenodeSelected", null, node);
		this._selectedNodes.push(node);
	},
	_onNodeUnselected: function(node){
		this._trigger("treenodeUnselected", null, node);
		this._selectedNodes = $.grep(this._selectedNodes, function(node){
			return !node.is(node);
		});
	},
	_onNodeClick: function(node){
		this._trigger("treenodeClick", null, node);
		$.each(this._selectedNodes, function(i, selectedNode){
            selectedNode.treenode("unselect");
		});
		this._selectedNodes = [];
	},
	_onNodeTreenodeDblclick: function(node){
		if(this.options.dblclickOpen){
			node.treenode("expand");
		}
	},
	_onNodeLoading: function(node){
		this._trigger("treenodeLoading", null, node);
	},
	getTopNodes: function(){
		return this.element.children(".ui-treenode");
	},
	getTopNode: function(){
		return this.element.children(".ui-treenode").eq(0);
	}
});

$.widget( "ui.tree", $.ui.treeBase, {
	_onNodeClick: function(node){
		this._trigger("treenodeClick", null, node);
	},
	_onNodeSelected: function(node){
        var self = this;
		if(this._selectedNode && !this._selectedNode.is(node)){
			this._selectedNode.treenode("unselect").unbind("treenoderemoved");
		}
		this._selectedNode = node;
        
        this._selectedNode.bind("treenoderemoved", function(event, removeNode){
            if(self._selectedNode.is(removeNode)){
			    self._selectedNode = null;
		    }
        })
		this._trigger("treenodeSelected", null, node);
	},
	_onNodeUnselected: function(node){
		if(this._selectedNode && this._selectedNode.is(node)){
			this._selectedNode = null;
		}
		this._trigger("treenodeUnselected", null, node);
	},
	deleteSelectedNode: function(){
		var self = this;
		var node = this.getSelectedNode();
		node.treenode("remove");
	},
	getSelectedNode: function(){
		return this._selectedNode;
	},
	expandTopNode: function(){
        var topNode = this.getTopNodes().eq(0);
		topNode.treenode("expand");
        return this;
	},
	selectTopNode: function(){
        var topNode = this.getTopNodes().eq(0);
		topNode.treenode("select");
        return this;
	}
});

$.widget( "ui.checkboxTree", $.ui.treeBase, {
	_isShowCheckbox: function(){
		return true;
	},
	getSelectedNodes: function(){
		return this._selectedNodes;
	}
});

$.widget( "ui.treenode", {
	_children: null,
	_childrenContainer: null, 
	_node: null,
	options: {
		text: null,
		iconClass: null,
		showCheckbox: null,
		expanded: false,
        loaded: false
	},
	_create: function() {
		var self = this;
		var node = this._node =  $("<div class='ui-treenode-node'><table cellspacing='0'><tr></tr></table></div>");
		var _childrenContainer = this._childrenContainer = $("<div class='ui-treenode-children'></div>");
		this.element.addClass("ui-treenode")
		.append(node)
		.append(_childrenContainer)
		.mouseover(function(){
			$(this).addClass("ui-treenode-node-hover");
		})
		.mouseout(function(){
			$(this).removeClass("ui-treenode-node-hover");
		});
		node.click(function(){
			self._onClick();
		});
		node.dblclick(function(){
			self._onDblclick();
		});
		
		this._renderSwitchCell();
		this._renderCheckboxCell();
		this._renderIconCell();
		this._renderContentCell();
	},
	_onClick: function(){
		this._trigger("click", null, this.element);
		this.select();
	},
	_onDblclick: function(){
		this._trigger("dblclick", null, this.element);
	},
	toggleSelected: function(){
		if(this.isSelected()){
			this.unselect();
		}else{
			this.select();
		}
	},
	isSelected: function(){
		return this._node.hasClass("ui-treenode-node-selected");
	},
	select: function(){
		if(this.options.showCheckbox){
			this._node.find(".ui-treenode-checkbox-cell input").prop("checked", true);;
		}
		this._node.addClass("ui-treenode-node-selected");
		this._trigger("selected", null, this.element);
	},
	unselect: function(){
		if(this.options.showCheckbox){
			this._node.find(".ui-treenode-checkbox-cell input").removeAttr("checked");
		}
		this._node.removeClass("ui-treenode-node-selected");
		this._trigger("unselected", null, this.element);
	},
	_renderSwitchCell: function(){
		var self = this;
		var switchCell = self._switchCell = $("<td><div class='ui-treenode-expand-icon'></div></td>");
		this._node.find("tr").append(switchCell);
		switchCell.click(function(ev){
			//is leaf
			if($(this).find("div").hasClass("ui-treenode-leaf-icon")){
				return;
			}
			
			if(self.options.expanded){
				self.collapse();
			}
			else{
				self.expand();
			}
			ev.stopPropagation();
		});
		if(this.options.expanded){
			this.expand();
		}
	},
	_renderCheckboxCell: function(){
		var self = this;
		if(this.options.showCheckbox){
			if(!this._checkboxCell){
				var td = this._checkboxCell = $("<td class='ui-treenode-checkbox-cell'><input type='checkbox'/></td>");
				this._switchCell.after(td);
				td.find("input").click(function(ev){
					if($(this).attr("checked") == "checked"){
						self.select();
					}
					else{
						self.unselect();
					}
					ev.stopPropagation();
				});
			}
		}
		else{
			if(this._checkboxCell){
				this._checkboxCell.remove();
				this._checkboxCell = null;
			}
		}
	},
	_renderIconCell: function(){
		var iconClass = this.options.iconClass;
		if(typeof  iconClass == "function"){
			iconClass = iconClass(this.element);
		}
		if(iconClass){
			if(this._iconCell == null){
				var iconCell = this._iconCell = $("<td><div></div></td>");
				this._node.find("tr").append(iconCell);
			}
			this._iconCell.find("div").removeClass().addClass(iconClass);
		}
		else{
			if(this._iconCell){
				this._iconCell.remove();
			}
		}
	},
	_renderContentCell: function(){
		if(this._contentCell == null){
			var contentCell = this._contentCell = $("<td><div class='ui-treenode-content' ></div></td>");
			this._node.find("tr").append(contentCell);
		}
		
		this._contentCell.find("div").text(this.options.text);
	},
	expand: function(){
		if(!this.options.loaded){
			this._trigger("loading", null, this.element);
		}
		this.options.expanded = true;
		if(!this._childrenContainer.children(".ui-treenode").length){
			this._switchCell.find("div").removeClass("ui-treenode-expand-icon").addClass("ui-treenode-leaf-icon");
			//this._childrenContainer.hide();
		}
		else{
			this._switchCell.find("div").removeClass("ui-treenode-expand-icon").addClass("ui-treenode-collapse-icon");
		}
		this._childrenContainer.show();
		this.options.loaded = true;
	},
	collapse: function(){
		this.options.expanded = false;
		this._switchCell.find("div").removeClass("ui-treenode-collapse-icon").addClass("ui-treenode-expand-icon");
		this._childrenContainer.hide();
	},
	append: function(data){
		var self = this;
		
		if(data.jquery){
			self._childrenContainer.append(data);
		}
		else{
			var treenode = $("<div></div>");
			self._childrenContainer.append(treenode);
			treenode.treenode(data);
			self._trigger("added", null, treenode);
		}
		if(this._switchCell.find("div").hasClass("ui-treenode-leaf-icon")){
			this._switchCell.find("div").removeClass("ui-treenode-leaf-icon").addClass("ui-treenode-collapse-icon");
		}
        this.options.loaded = true;
	},
	remove: function(){
		var self = this;
		this.empty();
		self._trigger("removed", null, this.element);
		this.element.remove();
	},
	empty: function(){
		var self = this;
		$.each(this.getChildren(), function(i, child){
			$(child).treenode("remove");
		});
        this.options.loaded = false;
	},
    reload: function(){
        this.empty();
        this.expand();
    },
	getParent: function(){
		return this.element.parent(".ui-treenode-children").parent();
	},
	getChildren: function(){
		return this._childrenContainer.children(".ui-treenode");
	},
	_setOption: function(key, value){
		$.Widget.prototype._setOption.apply( this, arguments );
		switch(key){
			case "text": this._renderContentCell(); break;
			case "iconClass": this._renderIconCell(); break;
			case "showCheckbox": this._renderCheckboxCell(); break;
			case "expanded": 
				if(this.options.expanded){
					this.expand();
				} 
				else{
					this.collapse();
				}
				break;
		}
	}
});

}( jQuery ) );