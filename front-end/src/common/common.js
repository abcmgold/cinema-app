/**
 * Format tiền
 * Author: BATUAN (28/05/2023) 
 */
export function formatMoney(money) {
    try {
        if (money) {
            money = parseFloat(money).toFixed(0);
            money = money.toString();
            return money.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        } else {
            return 0;
        }

    } catch (error) {
        return error
    }
}

/**
 * Bỏ format tiền
 * Author: BATUAN (28/05/2023) 
 */
export function unformatMoney(money) {
    try {
        if (money) {
            var value = parseInt(money.replaceAll('.', ''));
            return value;
        } else {
            return '';
        }
    } catch (error) {
        console.log(error);
    }
}

/**
 * Format ngày tháng
 * Author: BATUAN (29/05/2023) 
 */
export function formatCurrentDate(date) {
    const day = date.getDate();
    const formattedDay = day.toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
    });

    const month = date.getMonth() + 1;
    const formattedMonth = month.toLocaleString('en-US', {
        minimumIntegerDigits: 2,
        useGrouping: false
    });
    const year = date.getFullYear();

    return `${year}-${formattedMonth}-${formattedDay}`
}

/**
 * Format tỷ lệ
 * Author: BATUAN (29/05/2023) 
 */
export function formatRatio(number) {
    if (! number) {
        return 0;
    }
    let formatted_number = new Intl.NumberFormat('vi-VN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(number);
    return formatted_number
}

export function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

export function formatDateTime(date) { // Chuỗi ban đầu

    // Chuyển chuỗi thành đối tượng Date
    var dateObject = new Date(date);

    // Định dạng lại thành giờ phút giây ngày tháng năm
    var gio = dateObject.getHours().toString().padStart(2, '0');
    var phut = dateObject.getMinutes().toString().padStart(2, '0');
    var giay = dateObject.getSeconds().toString().padStart(2, '0');
    var ngay = dateObject.getDate().toString().padStart(2, '0');
    var thang = (dateObject.getMonth() + 1).toString().padStart(2, '0'); // Lưu ý tháng trong JavaScript đếm từ 0 đến 11
    var nam = dateObject.getFullYear();

    var ket_qua = gio + ":" + phut + ":" + giay + " " + ngay + "/" + thang + "/" + nam;
    return ket_qua
}
