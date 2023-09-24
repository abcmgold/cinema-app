import ENUM from "./enum";
function exception(statusCode, message, showToastError) {
    switch (statusCode) {
        case ENUM.statusCode.badRequest:
            message.forEach((mess) => {
                showToastError(mess);
            });
            break;
        case ENUM.statusCode.conflic:
            showToastError(message);
            break;
        case ENUM.statusCode.serverError:
            showToastError(message);
            break;
        default:
            showToastError(message);
            break;
    }
}
export default exception;