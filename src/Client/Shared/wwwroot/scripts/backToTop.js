﻿let timeoutID;
let oldScrollY = 0;

function OnScrollEvent() {
    const artifactListDiv = document.querySelector('.list-container');
    artifactListDiv.scrollTop = 0;
}


function HideBackToTopButton() {
    {
        var scrollButton = document.getElementsByClassName('position-scroll-btn')[0];
        scrollButton.style.display = 'none';
        if (typeof timeoutID != undefined) {
            clearTimeout(timeoutID);
            timeoutID = undefined;
        }
    }
}

function ShowBackToTopButton() {
    var scrollButton = document.getElementsByClassName('position-scroll-btn')[0];
    scrollButton.style.display = 'block';
}

// array for store scroll top value for back button
let savePositionScroll = [];

function getLastScrollPosition() {
    const artifactListDiv = document.querySelector('.list-container');
    let lastScrollPosition = savePositionScroll[savePositionScroll.length - 1];
    artifactListDiv.scrollTop = lastScrollPosition;
    savePositionScroll.pop();
}

function saveScrollPosition() {
    const artifactListDiv = document.querySelector('.list-container');
    savePositionScroll.push(artifactListDiv.scrollTop);
}

function SearchInputUnFocus() {
    var searchInput = document.getElementById('searchinput');
    var searchIcon = document.querySelector('.search-icon');
    if (typeof searchInput != undefined) {
        searchInput.blur();
        searchIcon.classList.remove("search-icon-active");
    }
}

function hammerIt(elm) {
    hammertime = new Hammer(elm, {});
    hammertime.get('pinch').set({
        enable: true
    });
    var posX = 0,
        posY = 0,
        scale = 1,
        last_scale = 1,
        last_posX = 0,
        last_posY = 0,
        max_pos_x = 0,
        max_pos_y = 0,
        transform = "",
        el = elm;

    hammertime.on('doubletap pan pinch panend pinchend', function(ev) {
        if (ev.type == "doubletap") {
            transform =
                "translate3d(0, 0, 0) " +
                "scale3d(3, 3, 1) ";
            scale = 3;
            last_scale = 3;
            try {
                if (window.getComputedStyle(el, null).getPropertyValue('-webkit-transform').toString() != "matrix(1, 0, 0, 1, 0, 0)") {
                    transform =
                        "translate3d(0, 0, 0) " +
                        "scale3d(1, 1, 1) ";
                    scale = 1;
                    last_scale = 1;
                }
            } catch (err) {}
            el.style.webkitTransform = transform;
            transform = "";
            last_posX = 0;
            last_posY = 0;
        }

        //pan    
        if (scale != 1) {
            posX = last_posX + ev.deltaX;
            posY = last_posY + ev.deltaY;
            max_pos_x = Math.ceil((scale - 1) * el.clientWidth / 2);
            max_pos_y = Math.ceil((scale - 1) * el.clientHeight / 2);
            if (posX > max_pos_x) {
                posX = max_pos_x;
            }
            if (posX < -max_pos_x) {
                posX = -max_pos_x;
            }
            if (posY > max_pos_y) {
                posY = max_pos_y;
            }
            if (posY < -max_pos_y) {
                posY = -max_pos_y;
            }
        }


        //pinch
        if (ev.type == "pinch") {
            scale = Math.max(.999, Math.min(last_scale * (ev.scale), 4));
        }
        if(ev.type == "pinchend"){last_scale = scale;}

        //panend
        if(ev.type == "panend"){
            last_posX = posX < max_pos_x ? posX : max_pos_x;
            last_posY = posY < max_pos_y ? posY : max_pos_y;
        }

        if (scale != 1) {
            transform =
                "translate3d(" + posX + "px," + posY + "px, 0) " +
                "scale3d(" + scale + ", " + scale + ", 1)";
        }

        if (transform) {
            el.style.webkitTransform = transform;
        }
    });
}

function InlineSearchInputUnFocus() {
    var InlinesearchInput = document.getElementById('inline-search-input');

    if (typeof InlinesearchInput != undefined) {
        InlinesearchInput.blur();
    }
}

function ImagePinchZoom() {
    let el = document.querySelector('#image-viewer');
    hammerIt(el);
}

function breadCrumbStyle() {
    let breadcrumbs = document.querySelector(".fx-breadcrumbs");

    if (!breadcrumbs)
        return;

    let startEllipsis = document.querySelector(".start-ellipsis");
    let endEllipsis = document.querySelector(".end-ellipsis");
    let hasHorizontalScrollbar = breadcrumbs.scrollWidth > breadcrumbs.clientWidth;

    if (hasHorizontalScrollbar) {
        startEllipsis.style.display = "block";
        breadcrumbs.scrollLeft = breadcrumbs.scrollWidth + breadcrumbs.scrollLeft;
    }

    breadcrumbs.addEventListener("scroll", () => {
        startEllipsis.style.display = "block";
        let breadcrumbsScroll = breadcrumbs.scrollLeft.toFixed();

        if (breadcrumbsScroll != 0) {
            startEllipsis.classList.add("color-changer");
            startEllipsis.style.display = "block";
            endEllipsis.classList.add("color-changer");

        } else {
            startEllipsis.classList.remove("color-changer");
            startEllipsis.style.display = "none";
        }

        if (breadcrumbs.offsetWidth + breadcrumbs.scrollLeft >= breadcrumbs.scrollWidth - 1) {
            endEllipsis.classList.remove("color-changer");
        }
    });
}

function breadCrumbStyleSelectionModal() {
    let breadcrumbs = document.querySelector(".sheet-wrapper .fx-breadcrumbs");

    if (!breadcrumbs)
        return;

    let startEllipsis = document.querySelector(".sheet-wrapper .start-ellipsis");
    let endEllipsis = document.querySelector(".sheet-wrapper .end-ellipsis");
    let hasHorizontalScrollbar = breadcrumbs.scrollWidth > breadcrumbs.clientWidth;

    if (hasHorizontalScrollbar) {
        startEllipsis.style.display = "block";
        breadcrumbs.scrollLeft = breadcrumbs.scrollWidth + breadcrumbs.scrollLeft;
    }

    breadcrumbs.addEventListener("scroll", () => {
        startEllipsis.style.display = "block";
        let breadcrumbsScroll = breadcrumbs.scrollLeft.toFixed();

        if (breadcrumbsScroll != 0) {
            startEllipsis.classList.add("color-changer");
            startEllipsis.style.display = "block";
            endEllipsis.classList.add("color-changer");

        } else {
            startEllipsis.classList.remove("color-changer");
            startEllipsis.style.display = "none";
        }

        if (breadcrumbs.offsetWidth + breadcrumbs.scrollLeft >= breadcrumbs.scrollWidth - 1) {
            endEllipsis.classList.remove("color-changer");
        }
    });
}


function OnScrollCheck() {
    const artifactListDiv = document.querySelector('.list-container');

    artifactListDiv.addEventListener("scroll", () => {
        const pinListDiv = document.querySelector('.pin-artifacts');
        if (typeof pinListDiv != undefined && pinListDiv !== null) {

            if (oldScrollY < artifactListDiv.scrollTop && artifactListDiv.scrollTop >= 350) {

                pinListDiv.classList.add('pin-artifacts-hide');
                pinListDiv.classList.remove('pin-artifacts-show');
                console.log("down");

            } else if (oldScrollY > artifactListDiv.scrollTop) {

                pinListDiv.classList.add('pin-artifacts-show');
                pinListDiv.classList.remove('pin-artifacts-hide');
                console.log("up");
            }

            oldScrollY = artifactListDiv.scrollTop;
        }

        if (artifactListDiv.scrollTop > 85) {
            ShowBackToTopButton();

            if (typeof timeoutID === 'undefined') {
                timeoutID = setTimeout(HideBackToTopButton, 3000);
            }
        } else {
            HideBackToTopButton();
            if (typeof timeoutID != undefined) {
                clearTimeout(timeoutID);
                timeoutID = undefined;
            }
        }
    });
}

function scrollToItem(itemId, listHeight,list) {
    let item = document.getElementById(itemId.toString());
    if (typeof list !== 'undefined' || list !== null) {
        list.scrollTop = listHeight;
        if (typeof item !== 'undefined' || item !== null) {
            try {
                addGrayBackground(item);
                return true;
            } catch (e) {
                return false;
            }
        }
        return false;
    }
}

function addGrayBackground(item) {
    item.classList.add('on-scroll-item-gray-background');
}