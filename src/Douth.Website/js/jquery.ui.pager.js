(function($){
    $.widget("ui.pager", {
            options: {
                pageInfo: {start: 0, size: 1, count: 1}
	        },
	        _create: function(){
                this.lblInfo = $("<span></span>");
                this.lnkPrevious = $("<a style='margin:0 .3em;' href='#'></a>").text("上一页");
                this.lnkNext = $("<a style='margin:0 .3em;' href='#'></a>").text("下一页");
                this.txtGo = $("<input style='width: 2em;margin:0 .3em;'/>");
                this.lblIndex = $("<span style='margin:0 .3em;'></span>");
                this.element.addClass("ui-widget")
                 .append(this.lblInfo)
                 .append(this.lnkPrevious)
                 .append(this.lnkNext)
                 .append(this.txtGo)
                 .append(this.lblIndex);

                var thiz = this;
                this.lnkPrevious.click(function(){
                    var start = thiz.options.pageInfo.start - thiz.options.pageInfo.size;
                    if(start < 0){
                        return;
                    }
    	            thiz._change(start);
                    return false;
    	        });
                this.lnkNext.click(function(){
                    var start = thiz.options.pageInfo.start + thiz.options.pageInfo.size;
                    if(start >= thiz.options.pageInfo.count){
                        return;
                    }
    	            thiz._change(start);
                    return false;
    	        });
                this.txtGo.change(function(){
                    var index = parseInt($(this).val());
                    var start = (index - 1) * thiz.options.pageInfo.size;
                    if(start < 0){
                        start = 0;
                    }
                    if(start > thiz.options.pageInfo.count){
                        var pageCount = thiz._getPageCount();
                        start = (pageCount - 1) * thiz.options.pageInfo.size;
                    }
    	            thiz._change(start);
    	        });
    	        this._render();
	        },
	        _change: function(start){
	            this._trigger("change", null, {start: start, size: this.options.pageInfo.size});
	        },
            _render: function(){
                var _end = this.options.pageInfo.start + this.options.pageInfo.size;
                if(this.options.pageInfo.count < _end){
                    _end = this.options.pageInfo.count;
                }
                var _start = this.options.pageInfo.start + 1;
                if(this.options.pageInfo.count <= 0){
                    _start = 0;
                }
                this.lblInfo.text("{0}-{1}/{2}".replace("{0}", _start).replace("{1}", _end).replace("{2}", this.options.pageInfo.count));

                var index = parseInt(this.options.pageInfo.start / this.options.pageInfo.size) + 1;
                this.txtGo.val(index);

                var pageCount = this._getPageCount();
                this.lblIndex.text("/" + pageCount);

                if(_start == 1){
                    this.lnkPrevious.hide();
                }
                else{
                    this.lnkPrevious.show();
                }

                if(_end == this.options.pageInfo.count){
                    this.lnkNext.hide();
                }
                else{
                    this.lnkNext.show();
                }
            },
            _getPageCount: function(){
                if(this.options.pageInfo.size <= 0){
                    return 0;
                }
                var pageCount = parseInt(this.options.pageInfo.count / this.options.pageInfo.size);
                if((this.options.pageInfo.count % this.options.pageInfo.size) > 0){
                    pageCount = pageCount + 1;
                }
                return pageCount;
            },
            _setOption: function (key, value) {
                $.Widget.prototype._setOption.apply( this, arguments );
                if(key == 'pageInfo'){
                    this._render();
                }
            }
        }
    );
})(jQuery);