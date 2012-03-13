/**
 * Created by Bodil.
 * User: Bodil
 * Date: 12-3-7
 * Time: 下午5:37
 */
var ConnectorClientPlugin = function(){};
ConnectorClientPlugin.prototype.connect = function (serverIp,port, succCallBack, failureCallBack) {
    return PhoneGap.exec(succCallBack,failureCallBack,'ConnectorClientPlugin','connect',[serverIp,port]);
};
ConnectorClientPlugin.prototype.send = function(messageStr,succCallBack,failureCallBack){
    return PhoneGap.exec(succCallBack,failureCallBack,'ConnectorClientPlugin','send',[messageStr]);
};
ConnectorClientPlugin.prototype.close = function(succCallBack,failureCallBack){
    return PhoneGap.exec(succCallBack,failureCallBack,'ConnectorClientPlugin','close',[]);
};

PhoneGap.addConstructor(function(){
    PhoneGap.addPlugin('ConnectorClientPlugin',new ConnectorClientPlugin());
});