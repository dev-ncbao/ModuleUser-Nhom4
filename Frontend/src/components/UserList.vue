<template>
    <div class="container mt-4">
        <h1>Users List</h1>
        <Router-link to="/user/add" class="btn btn-primary ">Add User</Router-link>
        <button v-on:click="logout" class="btn btn-secondary" style="margin-left: 70%;">Log out</button>
        <br><br>
        <table class="table">
            <thead>
                <tr class="table-dark">
                    <th scope="col">#</th>
                    <th scope="col">Username</th>
                    <th scope="col">Password</th>
                    <th scope="col">Name</th>
                    <th scope="col">Expire</th>
                    <th scope="col">&emsp;&emsp;&ensp;Action</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(value, index) in data" :key="index">
                    <th scope="row">{{ index + 1 }}</th>
                    <td>{{ value.username }}</td>
                    <td>{{ value.password }}</td>
                    <td>{{ value.name }}</td>
                    <td>{{ value.expire ? value.expire : "null" }}</td>
                    <td>
                        <Router-link :to="'/user/edit/' + value.username" class="btn btn-primary">Edit</Router-link>
                        &emsp;
                        <button class="btn btn-danger" v-on:click="deleteData(value.username)">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import { RouterLink } from 'vue-router';
import { api, api3 } from '../api';
export default {
    data() {
        return {
            data: null,
        }
    },
    mounted() {
        this.axios.get(api).then((res) => {
            if (res.data) {
                this.data = res.data;
            }
            /* const key = localStorage.getItem('key')
            if(!key){
                this.$router.push('/')
            } */
        })
    },
    methods: {
        deleteData(username) {
            const key = localStorage.getItem('username')
            if (key==username) {
                alert("Nguoi dung dang login!!!")
            }
            else{    
                if (username) {
                    this.axios.delete(`${api}/${username}`).then(res => {
                        if (res.status == 200) {
                            let newArr = this.data.filter(el => el.username !== username)
                            this.data = newArr
                        }
                    }).
                    catch(err => { alert("Có lỗi xảy ra")})
                }
            }
        },
        logout() {
            this.axios.put(`${api3}/${localStorage.getItem('username')}`).then(res => {
                        if (res.status == 200) {
                            localStorage.clear()
            this.$router.push('/')
                        }
                    }).catch(err => { alert("Có lỗi xảy ra")})
            
            // console.log(localStorage.getItem('key'))
        }
    }
}
</script>