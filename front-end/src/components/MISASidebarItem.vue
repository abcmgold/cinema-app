<template>
    <router-link :to = "router" class = "sidebar__item" :class = "activeClass">
        <div class = 'sidebar__item--img'>
            <m-icon :icon="icon" ></m-icon>
        </div>
        <div class="sidebar__item--text" >{{label}}</div>
        <m-icon :icon="arrow"></m-icon>
    </router-link>
</template>


<script>
export default {
    name: "MISASidebarItem",
    data() {
        return {
            route:''
        }
    },
    props: {
        label: String,
        icon: String,
        activeClass: String,
        router: {
            type: String,
            default: "/"
        },
        arrow: String
    },
    created() {
        this.route = this.router
    }
}
</script>

<style scoped>
@import url(@/css/components/icon.css);
</style>