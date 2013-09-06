(function( $, undefined ) {

var cloumnDefaultWidth = 100;

$.widget( "ui.datagrid", {
	_headerCells: null,
	_rows: null,
	options: {
		columns: null,
		singleSelect: false,
		width: null,
		height: null,
		data: null,
		showNumberColumn: false
	},
	_create: function() {
		var self = this;
		this._headerCells = [];
		this._rows = [];
		this.element.addClass("ui-datagrid");
		
		if(this.options.columns == null || !this.options.columns.length){
			return;
		}
		
	 	var header = this._header = $("<div class='ui-datagrid-header'><div class='ui-datagrid-header-content'><table class='table table-bordered'><thead><tr></tr></thead></table></div></div>");
		this.element.append(header);
		var headerContent = this._headerContent = header.children(".ui-datagrid-header-content");
		
		var body = this._body = $("<div class='ui-datagrid-body'><div class='ui-datagrid-body-content'></div></div>");
		var bodyContent = this._bodyContent = body.find(".ui-datagrid-body-content");
		body.scroll(function(){
			header.scrollLeft($(this).scrollLeft());
		});
		this.element.append(body);
		
		this._renderHeader();
		this._renderHeight();
        $(window).resize(function() {
            self._renderHeight();
        });
	},
	_init: function(){
		this._renderBody();
	},
	_renderHeader: function(){
		this._renderHeaderNumberCell();
		this._renderHeaderCheckboxCell();
		this._renderHeaderCells();
	},
	_renderHeaderNumberCell: function(){
		this._header.find(".ui-datagrid-header-numbercell").remove();
		if(this.options.showNumberColumn){
			var th = $("<th class='ui-datagrid-header-numbercell' style='width:30px'></th>");
			this._header.find("tr").append(th);
		}
	},
	_renderHeaderCheckboxCell: function(){
		var self = this;
		this._header.find(".ui-datagrid-header-checkbox-cell").remove();
		if(!this.options.singleSelect){
			var th = $("<th class='ui-datagrid-header-checkbox-cell' style='width:20px'><input type='checkbox'/></th>");
			th.click(function(ev){
				self._onHeaderCheckboxCell_click($(this));
				ev.stopPropagation();
			});
			this._header.find("tr").append(th);
		}
	},
	_renderHeaderCells: function(){
		var self = this;
		this._headerCells = [];
		this._header.find(".ui-datagrid-header-cell").remove();
		$.each(this.options.columns, function(i, column){
			var th = $("<th class='ui-datagrid-header-cell'></th>");
			if(column.sortDirection === "desc"){
				th.append("<div class='ui-datagrid-header-sort-icon'>▼</div>");
			}
			else if(column.sortDirection === "asc"){
				th.append("<div class='ui-datagrid-header-sort-icon'>▲</div>");
			}
			th.append("<div class='ui-datagrid-header-title'>" + column.title + "</div>");
			if(column.width){
				th.width(column.width);
			}
			else{
				th.width(cloumnDefaultWidth);
			}
			self._header.find("tr").append(th);
			var cell = {element: th, column: column};
			self._headerCells.push(cell);
			th.click(function(){
				if(self.options.canSort){
					self.sortBy(column.name, self._toggleDirection(column.sortDirection));
				}
			});
		});
		this._renderWidth();
	},
	_toggleDirection: function(direction){
		if(direction === "desc"){
			return "asc";
		}else{
			return "desc";
		}
	},
	sortBy: function(columnName, direction){
		var cells = $.grep(this._headerCells, function(cell, i){
			return cell.column.name === columnName;
		});
		if(cells && cells.length){
			this._header.find(".ui-datagrid-header-sort-icon").remove();
			var cell = cells[0];
			cell.column.sortDirection = direction;
			if(direction === "desc"){
				cell.element.prepend("<div class='ui-datagrid-header-sort-icon'>▼</div>");
			}
			else if(direction === "asc"){
				cell.element.prepend("<div class='ui-datagrid-header-sort-icon'>▲</div>");
			}
			this._trigger("sort", null, cell);
		}
	},
	_renderHeight: function(){
        var height;
        if(this.options.height == "auto"){
            var documentHeight = $(window).height();
            var bodyPaddingHeight = $(document.body).innerHeight() - $(document.body).height();
            var parentHeight = this.element.parent().height() - this.element.height();
            height  = documentHeight - parentHeight - bodyPaddingHeight;
        }
		else if(this.options.height){
            height = this.options.height;
		}

        if(height){
            var headerHeight = this._header.height();
		    if(height >= headerHeight){
			    this._body.height(height - headerHeight);
		    }
        }
	},
	_renderWidth: function(){
		if(this.options.width){
			this._header.width(this.options.width);
			this._body.width(this.options.width);
			this._headerContent.width(this.options.width);
			var headerTableWidth = this._headerContent.find("table").width();
			if(this._headerContent.width() < headerTableWidth + 30){
				this._headerContent.width(headerTableWidth + 30);
			}
		}
	},
	_renderBody: function(){
		var self = this;
		$.each(this._rows, function(i, row){
			self.deleteRow(row)
		});
		this._rows = [];
		if(this.options.data && this.options.data.length){
			$.each(this.options.data, function(i, data){
				self.appendRow(data);
			})
		}
	},
	appendRow: function(data){
		var self = this;
		var datarow = $("<div></div>");
		this._bodyContent.append(datarow);
		datarow.datarow({
			columns: this.options.columns,
			showNumberCell: this.options.showNumberColumn,
			showCheckboxCell: !this.options.singleSelect,
			data: data
		})
		.bind("datarowclick", function(evt, row){self.unselectAllRow();})
		.bind("datarowselected", function(evt, row){self._onDatarow_selected(row);})
		.bind("datarowunselected", function(evt, row){self._onDatarow_unselected(row);});

		this._rows.push(datarow);
		this._refreshOddRowClass();
		this._refreshNumberRow();
		this._trigger("addedRow", null, {row: datarow, data: data});
	},
	_onDatarow_selected: function(row){
		if(this._bodyContent.find(".ui-datagrid-selected").length == this._rows.length){
			this._header.find(".ui-datagrid-header-checkbox-cell input").attr("checked", "checked");
		}
		this._trigger("selectedRow", null, row);
	},
	_onDatarow_unselected: function(row){
		this._header.find(".ui-datagrid-header-checkbox-cell input").removeAttr("checked");
		this._trigger("unselectedRow", null, row);
	},
	_onHeaderCheckboxCell_click: function(cell){
		var self = this;
		if(cell.find("input").attr("checked") == 'checked'){
			this.selectAllRow();
		}
		else{
			this.unselectAllRow();
		}
	},
	selectAllRow: function(){
		var self = this;
		$.each(this._rows, function(i, row){
			row.datarow("select");
		})
	},
	unselectAllRow: function(){
		var self = this;
		$.each(this._rows, function(i, row){
			row.datarow("unselect");
		})
	},
	_refreshOddRowClass: function(){
		this._bodyContent.find(".ui-datagrid-oddrow").removeClass("ui-datagrid-oddrow");
		this._bodyContent.find(".ui-datagrid-row :odd").addClass("ui-datagrid-oddrow");
	},
	_refreshNumberRow: function(){
		if(this.options.showNumberColumn){
		 	var numberRows = this._bodyContent.find(".ui-datagrid-row tr td:first-child");
			for(var i = 0; i < numberRows.length; i++){
				numberRows.eq(i).text(i + 1);
			}
		}
	},
	updateRow: function(index, data){
		if(this._rows[index]){
			this._rows[index].datarow("option", "data", data);
			this._trigger("updatedRow", null, {row: this._rows[index], data: data});
		}
	},
	selectRow: function(index){
		if(this._rows[index]){
			this._rows[index].datarow("select");
		}
	},
	unselectRow: function(index){
		if(this._rows[index]){
			this._rows[index].datarow("unselect");
		}
	},
	getRows: function(){
		return this._rows;
	},
	getRowsData: function(){
		return $.map(this._rows, function(i, row){
			row.datarow("option", "data");
		});
	},
	deleteSelectedRow: function(){
		var self = this;
		var selectRows = this.getSelectedRow();
		$.each(selectRows, function(i, row){
			self.deleteRow(row)
		});
	},
	getSelectedRows: function(){
		var selectRows = $.grep(this._rows, function(row){
			return row.datarow("isSelected");
		});
		return selectRows;
	},
	getSelectedRow: function(){
		var selectRows = this.getSelectedRows();
        if(selectRows && selectRows.length){
            return selectRows[0];
        }
		return null;
	},
	deleteRow: function(deletedRow){
		if(typeof deletedRow == "number"){
			deletedRow = this._rows[deletedRow];
		}
		if(!deletedRow){
			return;
		}
		var self = this;
		var rows = [];
		$.each(this._rows, function(i, row){
			if(row != deletedRow){
				rows.push(row);
			}else{
				var data = row.datarow("option", "data");
				row.remove();
				self._trigger("deletedRow", null, data);
			}
		});
		this._rows = rows;
		this._header.find(".ui-datagrid-header-checkbox-cell input").removeAttr("checked");
		this._refreshOddRowClass();
		this._refreshNumberRow();
	},
	_setOption: function(key, value){
		var self = this;
		$.Widget.prototype._setOption.apply(self, arguments);
		switch(key){
			case "width": this._renderWidth();break;
			case "height": this._renderHeight();break;
			case "showNumberColumn": 
				this._renderHeaderNumberCell();
				$.each(this._rows, function(i, row){
					row.datarow("option", "showNumberCell", value);
				});
				break;
			case "singleSelect": 
				this._renderHeaderCheckboxCell();
				$.each(this._rows, function(i, row){
					row.datarow("option", "showCheckboxCell", !value);
				});
				break;
			case "data": this._renderBody();break;
			case "columns": 
				this._renderHeaderCells();
				$.each(this._rows, function(i, row){
					row.datarow("option", "columns", value);
				});
				break;
		}
	}
});

$.widget( "ui.datarow",{
	_cells: null,
	_selected: null,
	options: {
		columns: null,
		showNumberCell: null,
		showCheckboxCell: null,
		data: null
	},
	_create: function() {
		var self = this;
		this._cells = [];
		this._selected = false;
		this.element.addClass("ui-datagrid-row")
		.append("<table class='table table-bordered table-hover'><tr ></tr></table>")
		.mouseover(function(){
			$(this).addClass("ui-datagrid-hover");
		})
		.mouseout(function(){
			$(this).removeClass("ui-datagrid-hover");
		})
		.click(function(ev){
			self._onDataRow_click(this, ev);
		});
		
		this._render();
	},
	_render: function(){
		this._renderNumberCell();
		this._renderCheckboxCell();
		this._renderDataCells();
	},
	_renderDataCells: function(){
		var self = this;
		var data = this.options.data;
		this.element.find(".ui-datagrid-cell").remove();
		this._cells = [];
		$.each(this.options.columns, function(i, column){
			var cell = {};
			cell.column = column;
			var td = $("<td class='ui-datagrid-cell'></td>");
			self.element.find("tr").append(td);
			cell.element = td;
			self._renderCell(cell, data);
			self._cells.push(cell);
		});
	},
	_renderNumberCell: function(){
		this.element.find(".ui-datagrid-numbercell").remove();
		if(this.options.showNumberCell){
			var td = $("<td class='ui-datagrid-numbercell' style='width:30px'></td>");
			this.element.find("tr").append(td);
		}
	},
	_renderCheckboxCell: function(){
		var self = this;
		this.element.find(".ui-datagrid-checkbox-cell").remove();
		if(this.options.showCheckboxCell){
			var td = $("<td class='ui-datagrid-checkbox-cell' style='width:20px'><input type='checkbox'/></td>");
			td.click(function(ev){
				if(!$(ev.target).is("input")){
					if($(this).find("input").attr("checked") == "checked"){
						$(this).find("input").removeAttr("checked");
					}
					else{
						$(this).find("input").attr("checked", "checked");
					}
				}
				self._onCheckboxCell_click($(this), self.element);
				ev.stopPropagation();
			});
			self.element.find("tr").append(td);
		}
	},
	_renderCell: function(cell, data){
		var column = cell.column;
		var self = this;
		var fieldValue = null;
		cell.element.empty();
		if(column.field && data[column.field]){
			fieldValue = data[column.field];
		}
		if(column.render){
			var renderValue = column.render(self.element, {data: data, value: fieldValue} );
			if(typeof renderValue === "string"){
				cell.element.html(renderValue);
			}
			else if(typeof renderValue === "object"){
				cell.element.append(renderValue);
			}
		}
		else if(column.field){
			cell.element.html(fieldValue);
		}
		if(column.width){
			cell.element.width(column.width);
		}
		else{
			cell.element.width(cloumnDefaultWidth);
		}
	},
	_onCheckboxCell_click: function(sender, datarow){
		if(sender.find("input").attr("checked") == "checked"){
			this.select();
		}
		else{
			this.unselect();
		}
	},
	_onDataRow_click: function(){
		var self = this;
		this._trigger("click", null, this.element);
		this.select();
	},
	select: function(){
		if(this.options.showCheckboxCell){
			this.element.find(".ui-datagrid-checkbox-cell input").attr("checked", "checked");
		}
		this.element.addClass("ui-datagrid-selected");
		this._selected = true;
		this._trigger("selected", null, this.element);
	},
	unselect: function(){
		this.element.removeClass("ui-datagrid-selected")
		 .find(".ui-datagrid-checkbox-cell input").removeAttr("checked");
		this._selected = false;
		this._trigger("unselected", null, this.element);
	},
	_update: function(data){
		var self = this;
		$.each(this._cells, function(i, cell){
			self._renderCell(cell, data);
		})
	},
	isSelected: function(){
		return this._selected;
	},
	_setOption: function(key, value){
		var self = this;
		$.Widget.prototype._setOption.apply(self, arguments);
		switch(key){
			case "data":this._update(value);break;
			case "showNumberCell": this._renderNumberCell();break;
			case "showCheckboxCell": this._renderCheckboxCell();break;
			case "columns": this._renderDataCells();break;
		}
	}
});
}( jQuery ) );