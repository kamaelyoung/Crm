(function($){
    $.widget("ui.resetPasswordDialog", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this._modifyUrl = $.baseUrl + "Org/ResetPassword";
                this.element.find("form").validate({
                    sendForm : false,
                    onBlur: true,
                    onChange: true,
	                eachValidField : function() {
		                $(this).closest('.control-group').removeClass('error');
                        $(this).next('.help-inline').hide();
	                },
	                eachInvalidField : function() {
		                $(this).closest('.control-group').addClass('error');
                        $(this).next('.help-inline').show();
	                },
                    valid: function(){
                        var formValue = thiz.element.find("form").getFormValue();
                        formValue.userIds = thiz._userIds;
                        $.post(thiz._modifyUrl, formValue, function(model){
                            if(model.result == 0){
                                thiz._modal.modal("hide");
                                thiz._editedCallback(model.data);
                            }
                            else{
                                alert(model.message)
                            }
                        });
                    }
                });
                
	        },
            reset: function(userIds, callback){
                this._userIds = userIds;
                this._editedCallback = callback;
                this._modal.modal("show");
            }
        }
    );
})(jQuery);