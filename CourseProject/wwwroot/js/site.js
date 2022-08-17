function AddElement() {
    var propertyValue = $('#propertySelect').val()
    var div = document.createElement('div')
    var inputBlock = document.getElementById('inputBlock')
    div.innerHTML = GetElement(propertyValue)

    inputBlock.appendChild(div)
}
function RemoveElement(div) {
    var inputBlock = document.getElementById('inputBlock')
    
    var parent = div.parentNode.parentNode.parentNode.parentNode

    inputBlock.removeChild(parent)
}

function GetElement(propertyValue) {

    if (propertyValue == 0) {
        return '<div class="d-flex flex-column"><label>Enter number input name</label><div class="input-group"><input class="form-control" type="text" name="propertyNames" placeholder="Enter..."/><div class="input-group-append"><input hidden name="propertyTypes" value="Number_Input"/><button class="btn btn-outline-danger" onclick="RemoveElement(this)" type="button">Remove</button></div></div></div>'
    }
    else if (propertyValue == 1) {
        return '<div class="d-flex flex-column"><label>Enter text input name</label><div class="input-group"><input class="form-control" type="text" name="propertyNames" placeholder="Enter..."/><div class="input-group-append"><input hidden name="propertyTypes" value="Text_Input"/><button class="btn btn-outline-danger" onclick="RemoveElement(this)" type="button">Remove</button></div></div></div>'
    }
    else if (propertyValue == 2) {
        return '<div class="d-flex flex-column"><label>Enter text area name</label><div class="input-group"><input class="form-control" type="text" name="propertyNames" placeholder="Enter..."/><div class="input-group-append"><input hidden name="propertyTypes" value="Text_Area"/><button class="btn btn-outline-danger" onclick="RemoveElement(this)" type="button">Remove</button></div></div></div>'
    }
    else if (propertyValue == 3) {
        return '<div class="d-flex flex-column"><label>Enter date input name</label><div class="input-group"><input class="form-control" type="text" name="propertyNames" placeholder="Enter..."/><div class="input-group-append"><input hidden name="propertyTypes" value="Date_Input"/><button class="btn btn-outline-danger" onclick="RemoveElement(this)" type="button">Remove</button></div></div></div>'
    }
    else if (propertyValue == 4) {
        return '<div class="d-flex flex-column"><label>Enter checkbox name</label><div class="input-group"><input class="form-control" type="text" name="propertyNames" placeholder="Enter..."/><div class="input-group-append"><input hidden name="propertyTypes" value="Check_Box"/><button class="btn btn-outline-danger" onclick="RemoveElement(this)" type="button">Remove</button></div></div></div>'
    }
    return ''
}




$(document).on("click", "[type='checkbox']", function (e) {
    if (this.checked) {
        $('#checkHidden').attr("value", "true");
    } else {
        $('#checkHidden').attr("value", "false");}
});
