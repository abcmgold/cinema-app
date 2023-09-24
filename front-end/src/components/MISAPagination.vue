<template>
    <div class="paging-total">
        {{ this.MISAResource['vn-VI'].total }}:&nbsp;&nbsp;<b>{{ this.totalRecords }}</b>
        {{ this.MISAResource['vn-VI'].record }}
    </div>
    <div class="paging-list">
        <div class="paging-list-text">{{this.MISAResource['vn-VI'].rowPerPage}}</div>
        <m-combo-box class="combo-box--small" :dataSelect="this.dataSelect" v-model="this.size"></m-combo-box>
        <div class="pagination-block">
            <el-pagination
                layout="prev, pager, next,jumper"
                :total="this.numberPages"
                :pager-count="5"
                v-model:current-page="this.currentPage"
                @current-change="handleCurrentChange(this.currentPage)"
            />
        </div>
        <!-- <m-number-input :isRequired="false"></m-number-input> -->
    </div>
</template>

<script scoped>
import { MISAResource } from '@/common/resource';
export default {
    name: 'MISAPagination',
    props: {
        numberPages: Number,
        dataSelect: Array,
        pageSize: Number,
        totalRecords: Number,
    },
    data() {
        return {
            currentPage: 1,
            size: 0,
            MISAResource: MISAResource,
        };
    },
    created() {
        this.size = this.pageSize;
    },
    watch: {
        size: function (newValue) {
            this.$emit('changePageSize', newValue);
        },
    },
    methods: {
         /*
         * Sự kiện giá trị trang hiện tại thay đổi
         * Author: BATUAN (14/06/2023)
         */
        handleCurrentChange() {
            if (this.currentPage > 0) {
                this.$emit('changeCurrentPage', this.currentPage);
            }
          
        },
    },
};
</script>

<style>
@import url(../css/components/paging.css);
</style>
