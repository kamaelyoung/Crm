(function($){
    $.widget("ui.renwuSousuoPopover", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this.element.click(function(event){
                    event.stopPropagation();
                });
                this.element.find(".date").datepicker();
                this._form = this.element.find("form").eq(0);
                this._form.find(":submit").click(function(){
                    var formValue = thiz._form.getFormValue();
                    thiz._trigger("sousuohou", null, formValue);
                    thiz.element.hide();
                    return false;
                });
                this._form.find(".btnQuxiao").click(function(){
                    thiz.element.hide();
                });
                
                thiz.element.parent().click(function(event){
                    thiz.element.hide();
                });
	        }
        }
    );
})(jQuery);