import {createStore} from 'vuex'

// Create a new store instance.
const store = createStore({
    state() {
        return {isAuthenticated: false, isShowMask: false}
    },
    mutations: {
        login(state) {
            state.isAuthenticated = true;
        },
        logout(state) {
            state.isAuthenticated = false;
        },
        showMask(state) {
            state.isShowMask = true;
        },
        hideMask(state) {
            state.isShowMask = false;
        }
    },

    getters: {
        isAuthenticated: (state) => state.isAuthenticated,
        isShowMask: (state) => state.isShowMask,
    },

})

export default store
