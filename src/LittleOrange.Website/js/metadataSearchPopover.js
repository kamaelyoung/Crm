(function($){
    $.widget("ui.metadataSearchPopover", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._popover = this.element.find(".popover");
                this._popover.click(function(event){
                    event.stopPropagation();
                });
                this._form = this.element.find("form").eq(0);
                this._form.find(":submit").click(function(){
                    var serach = thiz._form.getSearch();
                    
                    if(thiz._cb){
                       thiz._cb(serach); 
                    }
                    thiz._popover.hide();
                    return false;
                });
                this._form.find(".btnCancel").click(function(){
                    thiz._popover.hide();
                });
                
                thiz.element.parent().click(function(event){
                    thiz._popover.hide();
                });
	        },
            search: function(of, cb){
                this._cb = cb;
                this._popover.show().position({my: "center top", at: "center bottom", of: of});
            }
        }
    );
})(jQuery);