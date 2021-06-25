function formatMoney(ip, amount) {
    try {
        var amt = amount.replaceAll(",", "");
        var num = new Number(amt).toLocaleString("en-US");
        document.getElementById(ip).value = num;
    } catch (e) {
        document.getElementById(ip) = amt;
    }
}

function changeFee(ip, value){
    const fee = value.split(',').join('');
    const vat = $("#vat").attr("id");
    try {
        let feeNumber = parseInt(fee);
        let vatNumber = ((feeNumber * 10) / 100).toString();

        formatMoney(ip, value);
        formatMoney(vat, vatNumber);
        let x = $("#vat").val();
        console.log(x);
    }
    catch (e) {
        document.getElementById(ip) = value;
    }
    
}

function formatMoney2(value1, value2) {
    let availBalance = $("#availBalance").attr("id");
    let balance = $("#balance").attr("id");
    try {
        formatMoney(availBalance, value1);
        formatMoney(balance, value2);
    }
    catch (e) {
        alert(e);
    }
}
