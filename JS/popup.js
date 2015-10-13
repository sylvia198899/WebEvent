function showMenu(menuNumber, eventObj) {
    //    alert(eventObj);
    hideAllMenus();
    var menuId = 'menu' + menuNumber;
    if (changeObjectVisibility(menuId, 'visible')) {
        var menuTitle = getStyleObject('menuTitle' + menuNumber);
        menuTitle.backgroundColor = '#ff9900';
        eventObj.cancelBubble = true;
        return true;
    } else {
        return false;
    }
}

var numMenus = 2;

function hideAllMenus() {
    for (counter = 1; counter <= numMenus; counter++) {
        changeObjectVisibility('menu' + counter, 'hidden');
        var menuTitle = getStyleObject('menuTitle' + counter);
        menuTitle.backgroundColor = '#000000';
    }
}

document.onclick = hideAllMenus;