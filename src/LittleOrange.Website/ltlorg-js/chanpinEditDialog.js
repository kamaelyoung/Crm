(function($){
    $.widget("ui.chanpinEditDialog", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this._form = this.element.find("form").eq(0);
                this.element.find(".btnSaveAndContinue").click(function(){

                });
                this.element.find(".btnSave").click(function(){
                    thiz._modal.modal("hide");
                });
                this._form.validate({
                    sendForm : false,
                    onBlur: true,
                    onChange: true,
				    eachValidField : function() {
					    $(this).closest('.control-group').removeClass('error');
				    },
				    eachInvalidField : function() {
					    $(this).closest('.control-group').addClass('error');
				    },
                    valid : function(event, options) {
                        var formValue = thiz._form.getFormValue();
                        thiz._editedCb(formValue);
                        thiz._form[0].reset();
                    }
                });
	        },
            edit: function(editedCb, initInfo){
                var thiz = this;
                thiz._editedCb = editedCb;
                this._modal.modal("show");
                if(initInfo){
                    thiz._form.setFormValue(initInfo);
                }
            }
        }
    );
})(jQuery);