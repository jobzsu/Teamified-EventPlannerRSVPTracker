import { toast } from "react-toastify";

var toastifyOptions = {
    closeButton: false,
    closeOnClick: true,
    draggable: true,
    hideProgressBar: true,
  };

const ToastifyHelper = {
  AlertDefaultNotif: (message, tId) => toast(message, { toastId: tId }),

  AlertInfoNotif: (message, tId) =>
    toast.info(message, { ...toastifyOptions, toastId: tId }),

  AlertSuccessNotif: (message, tId) =>
    toast.success(message, { ...toastifyOptions, toastId: tId }),

  AlertWarnNotif: (message, tId) =>
    toast.warn(message, { ...toastifyOptions, toastId: tId }),

  AlertErrorNotif: (message, tId) =>
    toast.error(message, { ...toastifyOptions, toastId: tId }),

  AlertProcessingNotif: (tId) =>
    toast("Processing", {
      isLoading: true,
      autoClose: false,
      closeOnClick: false,
      draggable: false,
      toastId: tId,
    }),

  SuccessProcessingNotif: (toastId, message) =>
    toast.update(toastId, {
      type: "success",
      isLoading: false,
      closeButton: false,
      closeOnClick: true,
      autoClose: 3000,
      draggable: true,

      render: message,
    }),

  WarningProcessingNotif: (toastId, message) =>
    toast.update(toastId, {
      type: "warning",
      isLoading: false,
      closeButton: false,
      closeOnClick: true,
      autoClose: 3000,
      draggable: true,

      render: message,
    }),

  ErrorProcessingNotif: (toastId, message) =>
    toast.update(toastId, {
      type: "error",
      isLoading: false,
      closeButton: false,
      closeOnClick: true,
      autoClose: 3000,
      draggable: true,

      render: message ?? "An error occurred while processing your request",
    }),
};

export default ToastifyHelper;
