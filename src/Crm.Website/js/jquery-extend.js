jQuery(function($){
	$.datepicker.regional['zh-CN'] = {
	    closeText: '关闭',
		prevText: '&#x3c;上月',
		nextText: '下月&#x3e;',
		currentText: '今天',
		monthNames: ['一月','二月','三月','四月','五月','六月',
		'七月','八月','九月','十月','十一月','十二月'],
		monthNamesShort: ['一','二','三','四','五','六',
		'七','八','九','十','十一','十二'],
		dayNames: ['星期日','星期一','星期二','星期三','星期四','星期五','星期六'],
		dayNamesShort: ['周日','周一','周二','周三','周四','周五','周六'],
		dayNamesMin: ['日','一','二','三','四','五','六'],
		dateFormat: 'yy-mm-dd', firstDay: 1,
		isRTL: false
		};
	$.datepicker.setDefaults($.datepicker.regional['zh-CN']);
});

(function($) {
	$.widget("ui.combobox", {
		_create: function() {
			var self = this;
			var select = this.element.hide();
			var input = $("<input>")
				.insertAfter(select)
				.autocomplete({
					source: function(request, response) {
						var matcher = new RegExp(request.term, "i");
						response(select.children("option").map(function() {
							var text = $(this).text();
							if (this.value && (!request.term || matcher.test(text)))
								return {
									id: this.value,
									label: text.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(request.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>"),
									value: text
								};
						}));
					},
					delay: 0,
					change: function(event, ui) {
						if (!ui.item) {
							// remove invalid value, as it didn't match anything
							$(this).val("");
							return false;
						}
						select.val(ui.item.id);
						self._trigger("selected", event, {
							item: select.find("[value='" + ui.item.id + "']")
						});
						
					},
					minLength: 0
				})
				.addClass("ui-widget ui-widget-content ui-corner-left ui-combobox-text");
			$("<button>&nbsp;</button>")
			.attr("tabIndex", -1)
			.attr("title", "Show All Items")
			.insertAfter(input)
			.button({
				icons: {
					primary: "ui-icon-triangle-1-s"
				},
				text: false
			}).removeClass("ui-corner-all")
			.addClass("ui-corner-right ui-button-icon ui-combobox-button")
			.click(function() {
				// close if already visible
				if (input.autocomplete("widget").is(":visible")) {
					input.autocomplete("close");
					return false;
				}
				// pass empty string as value to search for, displaying all results
				input.autocomplete("search", "");
				input.focus();
				return false;
			});
		}
	});
    

    $.widget(
        "ui.singleSelectUser", {
            options: {
            },
            _create: function(){
                var element = this.element;
                if($.debug){
                    element.find(".userName").removeAttr("readonly").val("qi");
                    element.find(".userAccount").show().val("qi");
                }
                else{
                    element.find(".userName").attr("readonly", "readonly");
                    element.find(".userAccount").hide();
                }
                element.find(":button").click(function(){
                    var url = edoc2BaseUrl + "/AppExt/Common/SelectOrgnization.aspx?userTree={show:true,multiSelect:" + false + ",current: true}&deptTree={show:false}";
                        var res = window.showModalDialog(url, "", "dialogWidth:750px; dialogHeight:450px;");
                        if (res != null && res.users != null) {
                            if(res.users.length > 0){
                                var user = res.users[res.users.length-1];
                                element.find(".userName").val(user._data.userRealName).focusout();
                                element.find(".userAccount").val(user._data.loginName);
                            }
                        }
                });
            }
        }
    );

    $.widget(
        "ui.multiSelectUser", {
            options: {
            },
            _create: function(){
                var element = this.element;
                var userAccount = element.find(".userAccount");
                var userName = element.find(".userName");
                if($.debug){
                    userAccount.show();
                    userName.removeAttr("readonly");
                }
                else{
                    userName.attr("readonly", "readonly");
                    userAccount.hide();
                }
                element.find(".selectButton").click(function(){
                    var url = edoc2BaseUrl + "/AppExt/Common/SelectOrgnization.aspx?userTree={show:true,multiSelect:true,current: true}&deptTree={show:false}";
                    var res = window.showModalDialog(url, "", "dialogWidth:750px; dialogHeight:450px;");
                    if (res != null && res.users != null) {
                        if(res.users.length > 0){
                            $.each(res.users, function(i, user){
                                if(userName.val()){
                                    userName.val(userName.val() + "," + user._data.userRealName).focusout();
                                }
                                else{
                                    userName.val(user._data.userRealName).focusout();
                                }
                                if(userAccount.val()){
                                    userAccount.val(userAccount.val() + "," + user._data.loginName);
                                }
                                else{
                                    userAccount.val(user._data.loginName);
                                }
                            })
                        }
                    }
                });
                element.find(".resetButton").click(function(){
                    userName.val("");
                    userAccount.val("");
                });
            }
        }
    );
    
    $.widget(
        "ui.userEmailMultiSelect", {
            options: {
            },
            _create: function(){
                var element = this.element;
                element.find(":button").click(function(){
                    var url = edoc2BaseUrl + "/AppExt/Common/SelectOrgnization.aspx?userTree={show:true,multiSelect:" + true + ",current: true}&deptTree={show:false}";
                        var res = window.showModalDialog(url, "", "dialogWidth:750px; dialogHeight:450px;");
                        var emailElement = element.find(".multiemail");
                        if (res != null && res.users != null) {
                            if(res.users.length > 0){
                                $.each(res.users, function(i, user){
                                    if(emailElement.val()){
                                        emailElement.val(emailElement.val() + ";" + user._data.email);
                                    }
                                    else{
                                        emailElement.val(user._data.email);
                                    }
                                })
                            }
                        }
                });
            }
        }
    );
})(jQuery);

(function($){$.fn.bgIframe=$.fn.bgiframe=function(s){if($.browser.msie&&/6.0/.test(navigator.userAgent)){s=$.extend({top:'auto',left:'auto',width:'auto',height:'auto',opacity:true,src:'javascript:false;'},s||{});var prop=function(n){return n&&n.constructor==Number?n+'px':n;},html='<iframe class="bgiframe"frameborder="0"tabindex="-1"src="'+s.src+'"'+'style="display:block;position:absolute;z-index:-1;'+(s.opacity!==false?'filter:Alpha(Opacity=\'0\');':'')+'top:'+(s.top=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+\'px\')':prop(s.top))+';'+'left:'+(s.left=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+\'px\')':prop(s.left))+';'+'width:'+(s.width=='auto'?'expression(this.parentNode.offsetWidth+\'px\')':prop(s.width))+';'+'height:'+(s.height=='auto'?'expression(this.parentNode.offsetHeight+\'px\')':prop(s.height))+';'+'"/>';return this.each(function(){if($('> iframe.bgiframe',this).length==0)this.insertBefore(document.createElement(html),this.firstChild);});}return this;};})(jQuery);

$(function(){if($.browser.msie && $.browser.version == "6.0")document.execCommand("BackgroundImageCache", false, true);});


jQuery.extend({
    rselect: /^(?:select)/i,
    rtextarea: /^(?:textarea)/i,
    rinput: /^(?:color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week|hidden)$/i,
    rradio: /^(?:radio)$/i,
    rcheckbox: /^(?:checkbox)$/i,
    getFormValue: function (form) {
        return $(form).getFormValue();
    }
});

jQuery.fn.extend({
    getViewColumn: function(){
        var columns = [];
        this.find("tbody tr").each(function(){
            var checkbox = $(this).find(":checkbox");
            if(checkbox.prop("checked")){
                var text = $(this).find(":text");
                var fieldId = $(this).data("fieldId");
                columns.push({fieldId: fieldId, width: text.val()});
            }
        })
        return columns;
    },
    setViewColumn: function(value){
        var columns = [];
        this.find("tbody tr").each(function(){
            var checkbox = $(this).find(":checkbox");
            var text = $(this).find(":text");
            var fieldId = $(this).data("fieldId");
            if(value[fieldId]){
                checkbox.prop("checked", true);
                text.val(value[fieldId].width);
            }
        })
        return columns;
    },
    getSearch: function(){
        var serach = {};
        this.find(".keywordSearch")
        .each(function (i, elem) {
            serach[elem.name] = $(elem).val();
					
		});

        this.find(".numberSearch, .dateSearch")
        .each(function (i, elem) {
            var value = $(this).getFormValue();
            var fieldCode = $(this).data("fieldCode");
            serach[fieldCode] = value;
					
		});

        var keyword = this.find(".keyword").val();
        serach.keyword = keyword;
        return serach;
    },
    setSearch: function(serach){
        if(!serach){
            return;
        }
        this.find(".keywordSearch")
        .each(function (i, elem) {
            $(elem).val(serach[elem.name]);
		});

        this.find(".numberSearch, .dateSearch")
        .each(function (i, elem) {
            var fieldCode = $(this).data("fieldCode");
            if(fieldCode in serach){
                $(this).setFormValue(serach[fieldCode]);
            }
		});
        return this;
    },
    getFormValue: function () {
        var valueArray = this.find(":input")
		.filter(function () {
		    return this.name &&
				(this.checked || $.rselect.test(this.nodeName) || $.rtextarea.test(this.nodeName) ||
					$.rinput.test(this.type));
		})
		.map(function (i, elem) {
		    var val = jQuery(this).val();

		    return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map(val, function (val, i) {
					    return { name: elem.name, value: val.replace(/\r?\n/g, "\r\n"), type: elem.type };
					}) :
					{ name: elem.name, value: val.replace(/\r?\n/g, "\r\n"), type: elem.type };
		}).get();

        var valueObj = {};
        
        $.each(valueArray, function (i, value) {
            if($.rcheckbox.test(value.type)){
                if(!valueObj[value.name]){
                    valueObj[value.name] = [];
                }
                valueObj[value.name].push(value.value);
            }
            else{
                valueObj[value.name] = value.value;
            }
        });
        return valueObj;
    },
    setFormValue: function (obj) {
        this.find(":input")
        .filter(function () {
            return this.name &&
				($.rradio.test(this.type) || $.rselect.test(this.nodeName) || $.rtextarea.test(this.nodeName) ||
					$.rinput.test(this.type) || $.rcheckbox.test(this.type));
        })
        .each(function () {
            if($.rradio.test(this.type)){
                if(obj[this.name] && $(this).val() == obj[this.name].toString()){
                    $(this).prop("checked", true);
                }
            }
            else if($.rcheckbox.test(this.type)){
                if($.isArray(obj[this.name])){
                    if(obj[this.name] && $.inArray($(this).val(), obj[this.name]) > -1){
                        $(this).prop("checked", true);
                    }
                    else{
                        $(this).prop("checked", false);
                    }
                }
            }
            else{
                $(this).val(obj[this.name]);
            }
        });
        return this;
    },
    setFormReadOnly: function () {
        this.map(function () {
            return this.elements ? jQuery.makeArray(this.elements) : this;
        })
        .filter(function () {
            return this.name &&
				($.rradio.test(this.type) || $.rselect.test(this.nodeName) || $.rtextarea.test(this.nodeName) ||
					$.rinput.test(this.type));
        })
        .each(function () {
            if ($.rradio.test(this.type) || $.rselect.test(this.nodeName)) {
                $(this).prop("disabled", true);
            }
            else {
                $(this).prop("readonly", true);
            }
        });
        return this;
    },
    validAndFocus: function () {
        var validValue = this.valid();
        if (!validValue) {
            this.validate().focusInvalid();
        }
        return validValue;
    },
    dateRange: function(){
        this.each(function(){
            var inputs = $(this).find("input");
            inputs.eq(0).datepicker({ changeMonth: true, changeYear: true, onSelect: function(selectedDate){
                inputs.eq(1).datepicker( "option", "minDate", selectedDate );
            }});
            inputs.eq(1).datepicker({ changeMonth: true, changeYear: true, onSelect: function(selectedDate){
                inputs.eq(0).datepicker( "option", "maxDate", selectedDate );
            }});
        }); 
    },
    setLoading: function(){
        if(!this.data("val")){
            this.data("val", this.val());
        }
        if(!this.data("loadingText")){
            this.data("loadingText", "loading...");
        }
        this.val(this.data("loadingText"));
        this.prop("disabled", true);
        return this;
    },
    resetLoading: function(){
        this.val(this.data("val"));
        this.prop("disabled", false);
        return this;
    }
});
$.ajaxSetup ({
    cache: false
});
$(document).ready(function() {

    $(".date").datepicker();
});