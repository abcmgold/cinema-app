import {createRouter, createWebHistory} from 'vue-router';
import MovieList from '../views/MovieList'
import MovieDetail from '../views/MovieDetail'
import InfoDetail from '../views/InfoDetail'
import InfoGeneral from '../views/infodetail/infodetailview/InfoGeneral'
import InfoTicket from '../views/infodetail/infodetailview/InfoTicket'

import MovieListViews from '../views/movies/MovieList'
import ScreenListViews from '../views/screens/ScreenList'
import ScreeningListViews from '../views/screenings/ScreeningList'
import EmployeeListViews from '../views/employees/EmployeeList'
import CinemaListViews from '../views/cinemas/CinemaList'
import TicketListViews from '../views/tickets/TicketList'
// import AdminContent from '../parts/admin/AdminContent'
import AdminLayout from '../layout/AdminLayout'
import CustomerLayout from '../layout/CustomerLayout'
import LoginLayout from '../layout/LoginLayout'
import SignupLayout from '../layout/SignupLayout'
import NotFound from '../layout/NotFound'

// định nghĩa các router
const routers = [
    {
        path: '/admin',
        component: AdminLayout,
        children: [
            {
                path: 'movie',
                component: MovieListViews
            },
            {
                path: 'screen',
                component: ScreenListViews
            },
            {
                path: 'cinema',
                component: CinemaListViews
            },
            {
                path: 'screening',
                component: ScreeningListViews
            }, {
                path: 'employee',
                component: EmployeeListViews
            }, {
                path: 'ticket',
                component: TicketListViews
            }
        ]
    },
    {
        path: '/notfound',
        component: NotFound
    },
    {
        path: '/login',
        component: LoginLayout
    },
    {
        path: '/signup',
        component: SignupLayout
    }, {
        path: '/',
        component: CustomerLayout,
        children: [
            {
                path: '',
                component: MovieList
            }, {
                path: 'customer',
                component: InfoDetail,
                children: [
                    {
                        path: 'general',
                        component: InfoGeneral
                    }, {
                        path: 'ticket',
                        component: InfoTicket
                    },

                ]
            }, {
                path: 'movies/:movieId',
                component: MovieDetail
            }
        ]
    }
]

// Khởi tạo router

const vueRouter = createRouter({history: createWebHistory(), routes: routers})

vueRouter.beforeEach((to, from, next) => { // Kiểm tra nếu đang ở đường dẫn kiểu '/admin'
    if (to.path.startsWith('/admin')) { // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
        if (!sessionStorage.getItem('jwtToken')) {
            next('/login');
        } else { // Kiểm tra vai trò của người dùng
            const userRole = sessionStorage.getItem('jwtToken') ? JSON.parse(atob(sessionStorage.getItem('jwtToken').split('.')[1])).role : null;

            // Kiểm tra nếu vai trò là 'admin', cho phép truy cập đến đường dẫn '/admin'
            // Nếu vai trò không phải 'admin', chuyển hướng đến trang không có quyền
            if (userRole === 'Admin') {
                next();
            } else {
                next('/notfound');
            }
        }
    }
    else if(to.path.startsWith('/customer')) {
        if (!sessionStorage.getItem('jwtToken')) {
            next('/login');
        }
        else {
            next();
        }
    } else { // Đối với các đường dẫn khác, cho phép truy cập bình thường
        next();
    }
});

export default vueRouter
