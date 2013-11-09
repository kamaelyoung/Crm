(function($){
    var chengyuanLeixing = {yonghu: 0, Yonghuzu: 1, Bumen: 2, Zhiwei: 3};

    $.widget("ui.chengyuanDialog", {
            options: {
                backdrop: true
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this._yonghuTab = this.element.find(".yonghuTab");
                this._yonghuzuTab = this.element.find(".yonghuzuTab");
                this._zhiweiTab = this.element.find(".zhiweiTab");
                this._bumenTab = this.element.find(".bumenTab");

                this.element.find(".yonghuPane").yonghuPane({tianjiahou: function(event, args){
                    if(thiz._danxuande){
                        thiz._chengyuanPane.chengyuanPane("clear");
                    }
                    thiz._chengyuanPane.chengyuanPane("tianjiaYonghu", args);
                }});
                this._chengyuanPane = this.element.find(".chengyuanPane").chengyuanPane();
                this.element.find(".btnXuanzeChengyuan").click(function(){
                    var list = thiz._chengyuanPane.chengyuanPane("getYonghuList");
                    if(!list.length){
                        alert("请选择用户");
                        return false;
                    }
                    var result;
                    if(thiz._onXuanzeYonghuhou){
                        result = thiz._onXuanzeYonghuhou(list);
                    }
                    else if(thiz._onXuanzehou){
                        result = thiz._onXuanzehou({yonghuList: list});
                    }
                    if(result){
                        thiz._modal.modal("hide");
                    }
                    return false;
                });
                this.element.modal({backdrop: this.options.backdrop, show: false});
	        },
            xuanze: function(yonghu, yonghuzu, zhiwei, bumen, onXuanzehou){
                this._onXuanzehou = onXuanzehou;
                if(yonghu != undefined && yonghu == false){
                    this._yonghuTab.hide();
                }
                if(yonghuzu!= undefined  && yonghuzu == false){
                    this._yonghuzuTab.hide();
                }
                if(zhiwei!= undefined  && zhiwei == false){
                    this._zhiweiTab.hide();
                }
                if(bumen!= undefined  && bumen == false){
                    this._bumenTab.hide();
                }
                if(yonghu){
                    this._yonghuTab.find("a").tab("show");  
                } 
                else if(yonghuzu){
                    this._yonghuzuTab.find("a").tab("show");
                }
                else if(zhiwei){
                    this._zhiweiTab.find("a").tab("show");
                }
                else if( bumen){
                    this._bumenTab.find("a").tab("show");
                }
                
                var modals = $(".modal.in").modal("hide");
                this._modal.on("hidden", function(){
                    modals.modal("show");
                });
                this._chengyuanPane.chengyuanPane("clear");
                this._modal.modal("show");  
            },
            xuanzeYonghu: function(onXuanzeYonghuhou, danxuande){
                this._onXuanzeYonghuhou = onXuanzeYonghuhou;
                this._danxuande = danxuande;
                this.xuanze(true, false, false, false);
            },
            xuanzeDangeYonghu: function(onXuanzeYonghuhou){
                this.xuanzeYonghu(onXuanzeYonghuhou, true);
            },
            guanbi: function(){
                this._modal.modal("hide");
            },
            _setOption: function (key, value) {
                $.Widget.prototype._setOption.apply( this, arguments );
                
            }
        }
    );

    $.widget("ui.yonghuPane", {
            options: {
                
	        },
	        _create: function(){
                this._bumenTree = this.element.find(".bumenTree");
                this._sousuoForm = this.element.find(".sousuoForm");
                this._yonghuGrid = this.element.find(".yonghuGrid");
                this._yonghuGridPager = this.element.find(".yonghuGridPager");
                var thiz = this;
                $.get($.baseUrl + "Chengyuan/DingjiBumen", null, function(resultModel){
                    if(resultModel.result == 0)
                    {
                        thiz._bumenTree.tree({
                            data: [resultModel.data],
		                    dblclickOpen: true,
		                    treenodeLoading: function(event, treenode){
                                thiz._jiazaiBumen(treenode);
		                    },
                            treenodeSelected: function(event, treenode){
                                var bumenId = treenode.treenode("option", "id");
                                thiz._jiazaiYonghu({bumenId: bumenId}, 0);
                            }
                        });
                        thiz._bumenTree.tree("expandTopNode").tree("selectTopNode");
                    }
                    else{
                        alert(resultModel.message);
                    }
                });

                this._sousuoForm.find("button").click(function(){
                    var formValue = thiz._sousuoForm.getFormValue();
                    var treenode = thiz._bumenTree.tree("getSelectedNode");
                    var bumenId = treenode.treenode("option", "id");
                    formValue.bumenId = bumenId;
                    thiz._jiazaiYonghu(formValue, 0);
                    return false;
                });

                this._yonghuGrid.datagrid({
                    columns:[
			            {title: "", width: 15, field:"zhanghao", render: function(datarow, args){
                            var yonghu = datarow.datarow("option", "data");
                            return $("<a href='#' style='font-weight:bold'>+</a>").click(function(){
                                thiz._trigger("tianjiahou", null, yonghu);
                                return false;
                            });
                        }},
			            {title: "帐号", width: 80, field:"account"},
			            {title: "姓名", width: 80, field:"name"},
			            {title: "职位", width: 120, field:"position"}
		            ],
		            canSort: false,
		            singleSelect: true,
		            showNumberRow: false,
                    height: 200
                });
                
                this._yonghuGridPager.pager({change: function(event, args){
                    thiz._jiazaiYonghu(thiz._sousuoXinxi, args.start);
                }});
	        },
            _jiazaiBumen: function(treenode){
                var bumenId = treenode.treenode("option", "id");
                $.get($.baseUrl + "Chengyuan/XiajiBumen", {bumenId: bumenId}, function(resultModel){
                    if(resultModel.result == 0){
                        if(resultModel.data && resultModel.data.length){
                            $.each(resultModel.data, function(i, nodeData){
                                treenode.treenode("append", nodeData).treenode("expand");
                            });
                        }
                    }
                    else{
                        alert(resultModel.message);
                    }
                });
            },
            _jiazaiYonghu: function(sousuoXinxi, start){
                var thiz = this;
                this._sousuoXinxi = sousuoXinxi;
                var args = {};
                $.extend(args, sousuoXinxi);
                args.start = start;
                args.size = 20;
                $.get($.baseUrl + "Chengyuan/SousuoYonghu", args, function(resultModel){
                    if(resultModel.result == 0){
                        thiz._yonghuGrid.datagrid("option", "data", resultModel.data.list);
                        thiz._yonghuGridPager.pager({pageInfo: {start: start, size: 20, count: resultModel.data.count}});
                    }
                    else{
                        alert(resultModel.message);
                    }
                });
            },
            _setOption: function (key, value) {
                $.Widget.prototype._setOption.apply( this, arguments );
                
            }
        }
    );

    $.widget("ui.chengyuanPane", {
            options: {
                
	        },
	        _create: function(){
                this._yonghuTab = this.element.find(".yonghuTab");
                this._yonghuzuTab = this.element.find(".yonghuzuTab");
                this._zhiweiTab = this.element.find(".zhiweiTab");
                this._bumenTab = this.element.find(".bumenTab");
                this.element.find(".yonghuPane").yonghuPane();
                this._yonghuList = [];
	        },
            tianjiaYonghu: function(yonghu){
                var thiz = this;
                var tianjiaguo = false;
                $.each(this._yonghuList, function(){
                    if(this.zhanghao == yonghu.zhanghao){
                        tianjiaguo = true;
                        return false;
                    }
                });
                if(tianjiaguo){
                    return;
                }
                var chengyuan = $("<span></span>").chengyuan({ id: yonghu.account, mingcheng: yonghu.name, leixing: chengyuanLeixing.yonghu, removed: function(){
                    thiz._yonghuList = $.grep(thiz._yonghuList, function(thisyonghu){
                        return thisyonghu.zhanghao != yonghu.zhanghao;
                    });
                }});
                this._yonghuList.push(yonghu);
                this.element.append(chengyuan);
            },
            getYonghuList: function(){
                return this._yonghuList;  
            },
            clear: function(){
                this.element.empty();
                this._yonghuList = [];  
            },
            _setOption: function (key, value) {
                $.Widget.prototype._setOption.apply( this, arguments );
                
            }
        }
    );

    $.widget("ui.yonghuXuanze", {
            options: {
                chengyuanDialog: null
	        },
	        _create: function(){
                var thiz = this;
                this._txtXingming = this.element.find(".xingming");
                this._txtZhanghao = this.element.find(".zhanghao");
                this._btnXuanze = this.element.find(".btnXuanze")
                .click(function(){
                    thiz.options.chengyuanDialog.chengyuanDialog("xuanzeDangeYonghu", function(yonghuList){
                        var yonghu = yonghuList[0];
                        thiz._txtXingming.val(yonghu.xingming);
                        thiz._txtZhanghao.val(yonghu.zhanghao);
                        return true;
                    });
                    return false;
                });

	        }
        }
    );
})(jQuery);