## Beacon @ RTS
### Question
+ When the bk.html will be called? A: by now we do not use bk.html
+ When script not enbaled?
+ Where log the user based beacon event to hbase? A: Yes

### Beacon Flow
User-->bk.js-->import javascript--> access bk.gif-->BeacanHandler

+ bid simulator http://127.0.0.1:8080/bidSimulator 
+ beacon generator http://jinjunmei.bnmat.cn/beaconGenerator?bi=5
+ beacon request http://jinjunmei.bnmat.cn/js/bk.js?ri=5

### Beacon Generator 
Generator the beacon code and send to website page owner
> BeaconGeratorServlet

> http://jinjunmei.bnmat.cn/beaconGenerator?bi=5
> Will set the yosemiteParas and yosemiteBkReq

```JAVASCRIPT
<script type='text/javascript'>
    var yosem1teParas = {
        extendedTracking: false,
        pub: 1
    };
    yosem1teBkReq = {
        ri: 5
    };
</script>
<script type='text/javascript' src='http://jinjunmei.bnmat.cn/js/bk.js?ri=5'></script>
<noscript>
    <img src='http://jinjunmei.bnmat.cn/bk.gif?ri=5' height=0 width=0 style='display:none' 
        alt='YosemiteCloud'/>
</noscript>
```

### Beacon Type
```SQL
(0, "LANDING"),
(1, "TRACKING"),
(2, "NEAR_CONVERSION"),
(3, "CONVERSION"),
(4, "DATA_TRANSFER");
```

### JavaScript of Beacon Request to handler
> Beacon Handler
> 
> http://jinjunmei.bnmat.cn/js/bk.js?ri=5
> 
> server/resources/adServingJavascript.js
> server/eventhandler/JsToServeBeaconHandler.java
```JAVASCRIPT
if (!window.yosem1tePreJavascript) {
    window.yosem1tePreJavascript = "pi>3.14";
    window.yosem1teAdServer = "jinjunmei.bnmat.cn";
}
"use strict"
if (!window.yosem1teAddEvent) {
    var yosem1teAddEvent = function(obj, type, fn) {
        if (obj.addEventListener) {
            obj.addEventListener(type, fn, false);
        } else if (obj.attachEvent) {
            obj["e" + type + fn] = fn;
            obj[type + fn] = function() {
                obj["e" + type + fn](window.event);
            };
            obj.attachEvent("on" + type, obj[type + fn]);
        }
    }
    var yosem1teRemoveEvent = function(obj, type, fn) {
        if (obj.removeEventListener) {
            obj.removeEventListener(type, fn, false);
        } else if (obj.detachEvent) {
            obj.detachEvent("on" + type, obj[type + fn]);
            obj[type + fn] = null;
            obj["e" + type + fn] = null;
        }
    }
    var yosem1teFindFlashVersion = function() {
        try {
            var nav, pins, val, i;
            nav = window.navigator;
            pins = nav.plugins;
            val = "";
            if (pins && pins.length) {
                for (i = 0; i < pins.length; i++) {
                    if (pins[i].name.indexOf('Shockwave Flash') != -1) {
                        val = pins[i].description.split('Shockwave Flash ')[1].split(" ")[0];
                        break;
                    }
                }
            } else {
                var suff = new Array(".7", "");
                for (i = 0; i < suff.length; i++) {
                    try {
                        var movie = new ActiveXObject("ShockwaveFlash.ShockwaveFlash" + suff[i]);
                        var arr = movie.GetVariable("$version").split(" ")[1].split(",");
                        val = arr[0] + "." + arr[1];
                        break;
                    } catch (e) {}
                }
            }
            return val;
        } catch (e) {
            return "";
        }
    }
    var yosem1teDef = function(o) {
        return o !== null && o !== undefined;
    }
    var yosem1teDebug = function(str) {
        return;
        if (console && console.debug) {
            console.debug(str);
        }
    };
    var yosem1teEscape = function(s) {
        return s === null || s === undefined ? "" : escape(s);
    }
    var yosem1teUrlProtocol = function(adRequestData) {
        var ptc = document.location.protocol;
        var secure = (yosem1teDef(adRequestData) && yosem1teDef(adRequestData.isSecureBidRequest)) ? (yosem1teAdReq.isSecureBidRequest === true || yosem1teAdReq.isSecureBidRequest === "true") : false;
        if (secure) {
            return "https://";
        } else {
            return ptc && ptc === "https:" ? "https://" : "http://";
        }
    }
    var yosem1teAdServe = function(paras, requestData) {
        return yosem1teAdBkServe(paras, requestData, true);
    }
    var yosem1teBkServe = function(paras, requestData) {
        return yosem1teAdBkServe(paras, requestData, false);
    }
    var yosem1teAdBkServe = function(paras, requestData, serveAd) {
        var i, a;
        if (!yosem1teDef(paras)) {
            return;
        }
        if (!window.yosem1tePreJavascript) {
            return;
        }
        var userUrl = window.location && window.location.href ? window.location.href : "";
        var toAppendToRedirects = "";
        var numAds = 0;
        var flash = yosem1teFindFlashVersion();
        var referrer = window.document && window.document.referrer ? window.document.referrer : "";
        yosem1teDoForExtendedTracking(paras, requestData);
        var clientTime = new Date().getTime();
        var randomStr = (clientTime % 1000000000) + "" + Math.random();
        var debug = requestData ? requestData.db : 0;
        var debug = 1;
        var suffix = "&cu=" + yosem1teEscape(userUrl) + "&rf=" + yosem1teEscape(referrer) + "&db=" + debug + "&rd=" + randomStr + "&pi =" + yosem1teEscape(paras.publisher) + "&fl=" + yosem1teEscape(flash) + "&ct=" + clientTime + "";
        if (serveAd) {
            if (requestData && requestData.aw > 0 && requestData.ah > 0 && requestData.pl > 0) {
                var newWinStr = requestData.nw ? "&newWin=1" : "";
                var placementStr = "&pl=" + yosem1teEscape(requestData.pl + "");
                var clickRedirect = yosem1teEscape(requestData ? requestData.clickRedirect : null) === "true";
                var clickPrefix = clickRedirect ? yosem1teEscape(requestData ? requestData.clickPrefix : null) : "";
                var bidderUrl;
                var bidTimeStamp;
                var adStr = "&ai=" + requestData.ai;
                var idStr = "&ri=" + requestData.ri;
                var priceStr = (yosem1teDef(requestData.pp)) ? "&pp=" + requestData.pp : "";
                var adSelectPath = "/as";
                var adSelectUrl = yosem1teUrlProtocol(requestData) + window.yosem1teAdServer + adSelectPath + "?aw=" + requestData.aw + "&ah=" + requestData.ah + priceStr + adStr + idStr + newWinStr + placementStr + "&cp=" + clickPrefix + suffix;
                adSelectUrl = adSelectUrl.length >= 4096 ? adSelectUrl.substr(0, 4095) : adSelectUrl;
                document.writeln("<script type='text/javascript'" + "src=\"" + adSelectUrl + "\"" + "></script>");
            }
        } else {
            var image = new Image(1, 1);
            var idStr = "&ri=" + requestData.ri;
            image.src = "http://" + window.yosem1teAdServer + "/bk.gif?" + idStr + suffix;
        }
    }
    var yosem1teDoForExtendedTracking, yosem1teDoOnAdSelectCallback;
    var yosem1teVisibilityCode = {
        FULL: 1,
        PARTIAL: 2,
        OFF_SCREEN: 3,
        UNKNOWN: 4
    };
    var yosem1teBeaconInterval = 5000;
    var yosem1teBeaconLimit = 121000;
    var yosem1teBeaconUrl = window.yosem1te_beacon_host;
    var yosem1teVisibilityCheckInterval = 500;
    var yosem1teGetAdUnitEl = function(containerId) {
        var c = document.getElementById(containerId);
        if (!c) {
            return null;
        }
        var ifr = c.getElementsByTagName("IFRAME")[0];
        return ifr;
    };
    var yosem1teFindElementPos = function(elem) {
        var left = 0,
            top = 0;
        if (elem.offsetParent) {
            while (elem.offsetParent) {
                left += elem.offsetLeft;
                top += elem.offsetTop;
                elem = elem.offsetParent;
            }
        }
        return {
            left: left,
            top: top
        };
    };
    var yosem1teFindScrollOffsets = function() {
        var offsetX = 0;
        var offsetY = 0;
        if (document.getElementById && !document.all) {
            offsetY = window.pageYOffset;
            offsetX = window.pageXOffset;
        } else {
            offsetY = document.documentElement.scrollTop;
            offsetX = document.documentElement.scrollLeft;
        }
        return {
            x: offsetX,
            y: offsetY
        };
    };
    var yosem1teFindClientDimensions = function() {
        var width = 0;
        var height = 0;
        if (typeof(window.innerWidth) == 'number') {
            width = window.innerWidth;
            height = window.innerHeight;
        } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
            width = document.documentElement.clientWidth;
            height = document.documentElement.clientHeight;
        } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
            width = document.body.clientWidth;
            height = document.body.clientHeight;
        }
        return {
            width: width,
            height: height
        };
    };
    var yosem1teGetElementVisibility = function(elemX, elemY, elemHeight, elemWidth, ignoreScroll) {
        var viewport = yosem1teFindClientDimensions();
        var scroll = ignoreScroll ? {
            x: 0,
            y: 0
        } : yosem1teFindScrollOffsets();
        var unitTop = elemY;
        var unitLeft = elemX;
        var unitBottom = elemY + elemHeight;
        var unitRight = elemX + elemWidth;
        var viewTop = scroll.y;
        var viewLeft = scroll.x;
        var viewBottom = scroll.y + viewport.height;
        var viewRight = scroll.x + viewport.width;
        if (unitTop >= viewTop && unitLeft >= viewLeft && unitBottom <= viewBottom && unitRight <= viewRight) {
            return yosem1teVisibilityCode.FULL;
        }
        if (unitLeft > viewRight || unitRight < viewLeft || unitTop > viewBottom || unitBottom < viewTop) {
            return yosem1teVisibilityCode.OFF_SCREEN;
        }
        return yosem1teVisibilityCode.PARTIAL;
    };
    var yosem1teSetCookie = function(name, value, days) {
        var expires;
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        } else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    };
    var yosem1teGetCookie = function(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1, c.length);
            }
            if (c.indexOf(nameEQ) == 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }
        return null;
    };
    var yosem1teDeleteCookie = function(name) {
        yosem1teSetCookie(name, "", -1);
    };
    (function() {
        var unitWidth = 0;
        var unitHeight = 0;
        var params = {};
        var yosem1teExtendedTracking = {
            detectScreenSettings: function() {
                params.sw = window.screen.availWidth;
                params.sh = window.screen.availHeight;
                params.cd = window.screen.colorDepth;
                params.pd = window.screen.pixelDepth;
            },
            detectVisibility: function() {
                var dimensions = yosem1teFindClientDimensions();
                params.pu = "0";
                if (window.self != window.top) {
                    params.pu = "1";
                }
                params.cw = dimensions.width;
                params.ch = dimensions.height;
                document.write('<div id="yosem1te-location-test"></div>');
                var testDiv = document.getElementById('yosem1te-location-test');
                var pos = yosem1teFindElementPos(testDiv);
                params.px = pos.left;
                params.py = pos.top;
                params.pv = yosem1teGetElementVisibility(params.positionX, params.positionY, unitHeight, unitWidth);
                params.hc = "0";
                var current = testDiv.parentNode;
                while (current && current.style) {
                    var v = current.style.visibility.toLowerCase();
                    var d = current.style.display.toLowerCase();
                    if (v == "hidden" || v == "collapse" || d == "none") {
                        params.hc = 1;
                        break;
                    }
                    current = current.parentNode;
                }
                testDiv.style.display = "none";
            },
            detectCookiesDisabled: function() {
                yosem1teSetCookie('yosem1teCookieTest', 1);
                if (!yosem1teGetCookie('yosem1teCookieTest')) {
                    params.cookie = 0;
                } else {
                    params.cookie = 1;
                    yosem1teDeleteCookie('yosem1teCookieTest');
                }
            },
            detectTabDepth: function() {
                params.hl = history.length;
            }
        };
        yosem1teDoForExtendedTracking = function(p, adReq) {
            unitWidth = adReq.aw;
            unitHeight = adReq.ah;
            if (p.extendedTracking) {
                for (var fn in yosem1teExtendedTracking) {
                    if (yosem1teDef(yosem1teExtendedTracking[fn])) {
                        try {
                            yosem1teExtendedTracking[fn]();
                        } catch (e) {
                            yosem1teDebug(e);
                        }
                    }
                }
            }
            p.clientContext = JSON.stringify(params);
        };
    })();
    (function() {
        var elem, adInstanceId;
        var canDetectVisibility = false;
        var totalTimeFullyVisible = 0;
        var totalTimePartiallyVisible = 0;
        var mouseOverStartTime = 0;
        var mouseOvers = 0;
        var mouseOuts = 0;
        var totalMouseOverTime = 0;
        var unitLoadTime = 0;
        var unitWidth, unitHeight;
        var yosem1teAfterServe = {
            recordUnitLoadTime: function() {
                unitLoadTime = new Date().getTime();
            },
            detectDimensions: function() {
                unitHeight = parseInt(elem.getAttribute('height') ? elem.getAttribute('height') : elem.style.height);
                unitWidth = parseInt(elem.getAttribute('width') ? elem.getAttribute('width') : elem.style.width);
                if (window.self != window.top) {
                    return;
                }
                canDetectVisibility = true;
            },
            initMouseovers: function() {
                yosem1teAddEvent(elem, 'mouseover', function() {
                    mouseOvers++;
                    mouseOverStartTime = new Date().getTime();
                    yosem1teDebug('Mouseovers: ' + mouseOvers);
                });
                yosem1teAddEvent(elem, 'mouseout', function() {
                    mouseOuts++;
                    totalMouseOverTime += new Date().getTime() - mouseOverStartTime;
                    mouseOverStartTime = 0;
                    yosem1teDebug('Mouseouts: ' + mouseOuts);
                    yosem1teDebug('Time spent hovering (seconds): ' + (totalMouseOverTime / 1000));
                });
            },
            initVisibilityCheck: function() {
                if (!canDetectVisibility) {
                    return;
                }
                window.setInterval(function() {
                    var p = yosem1teFindElementPos(elem);
                    positionX = p.left;
                    positionY = p.top;
                    var status = yosem1teGetElementVisibility(positionX, positionY, unitHeight, unitWidth);
                    if (status == yosem1teVisibilityCode.FULL) {
                        totalTimeFullyVisible += yosem1teVisibilityCheckInterval;
                    } else if (status == yosem1teVisibilityCode.PARTIAL) {
                        totalTimePartiallyVisible += yosem1teVisibilityCheckInterval;
                    }
                }, yosem1teVisibilityCheckInterval);
            },
            initBeacon: function() {
                var intervalId = window.setInterval(function() {
                    var t = totalMouseOverTime;
                    if (mouseOvers > mouseOuts) {
                        t += (new Date().getTime() - mouseOverStartTime);
                    }
                    var params = {
                        ai: adInstanceId,
                        pu: (canDetectVisibility ? "0" : "1"),
                        mo: mouseOvers.toString(),
                        mt: (t / 1000).toString(),
                        tt: ((new Date().getTime() - unitLoadTime) / 1000).toString(),
                        tf: (totalTimeFullyVisible / 1000).toString(),
                        tp: (totalTimePartiallyVisible / 1000).toString()
                    };
                    var pJson = JSON.stringify(params);
                    yosem1teDebug(pJson);
                    var img = new Image(1, 1);
                    img.src = yosem1teBeaconUrl + 'p=' + pJson;
                }, yosem1teBeaconInterval);
                window.setTimeout(function() {
                    window.clearInterval(intervalId);
                }, yosem1teBeaconLimit);
            }
        };
        yosem1teDoOnAdSelectCallback = function(aid) {
            if (yosem1teBeaconUrl) {
                adInstanceId = aid;
                elem = yosem1teGetAdUnitEl('__yosem1te');
                yosem1teAddEvent(elem, 'load', function() {
                    for (var fn in yosem1teAfterServe) {
                        if (yosem1teAfterServe[fn]) {
                            try {
                                yosem1teAfterServe[fn]();
                            } catch (e) {
                                yosem1teDebug(e);
                            }
                        }
                    }
                });
            }
        };
    })();
}
try {
    if (yosem1teBkReq !== undefined) {
        yosem1teBkServe(yosem1teParas, yosem1teBkReq);
    }
} catch (err) {};
```