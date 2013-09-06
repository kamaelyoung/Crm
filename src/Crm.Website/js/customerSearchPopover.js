﻿(function($){
    $.widget("ui.customerSearchPopover", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._popover = this.element.find(".popover");
                this._popover.click(function(event){
                    event.stopPropagation();
                });
                this.element.find(".date").datepicker();
                this._form = this.element.find("form").eq(0);
                this._form.find(":submit").click(function(){
                    var extendSearchs = thiz._form.find(".extendSearchs").getExtendSearchValue();
                    var keyword = thiz._form.find(".keyword").val();
                    if(thiz._cb){
                       thiz._cb({keyword: keyword, extendSearchs: extendSearchs}); 
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