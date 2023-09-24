const pressEscEvent = {
    beforeMount(el, binding) {
        el.pressEsc = event => {
            if (event.key === 'Escape') {
                binding.value()
            }
        }
        document.addEventListener('keydown', el.pressEsc)
    },
    unmounted(el) {
        if (el.pressEsc) {
            document.removeEventListener('keydown', el.pressEsc);
            delete el.pressEsc;
        }
    }
}

const leftClickMouse = {
    mounted(el, binding) {
        el.leftClickHandler = event => {
            if (event.button === 0) { // Kiểm tra nút chuột là nút trái (0)
                binding.instance.$store.commit('hideContextMenu');
            }
        };
        el.addEventListener('click', el.leftClickHandler);
    },
    unmounted(el) {
        if (el.leftClickHandler) {
            el.removeEventListener('click', el.leftClickHandler);
            delete el.leftClickHandler;
        }
    }
};


export {
    pressEscEvent,
    leftClickMouse
};
