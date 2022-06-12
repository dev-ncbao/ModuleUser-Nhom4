<template>
    <div class="container mt-4">
        <h1>Edit User</h1>
        <form>
            <div class="mb-3">
                <label for="un" class="form-label">User name</label>
                <input disabled v-model="username" type="email" class="form-control" id="un">
            </div>
            <div class="mb-3">
                <label for="n" class="form-label">Name</label>
                <input v-model="name" type="email" class="form-control" id="n">
            </div>
            <div class="mb-3">
                <label for="pw" class="form-label">Password</label>
                <input v-model="password" type="password" class="form-control" id="pw">
                <!-- <div id="emailHelp" class="form-text">Nhập sai ban nick :))</div> -->
            </div>
            <button type="submit" class="btn btn-primary" v-on:click="saveData">Update</button>
        </form>
    </div>
</template>

<script>
import { api } from '../api'
export default {
    data() {
        return {
            name: null,
            username: null,
            password: null
        }
    },
    mounted() {
        let { username } = this.$route.params;
        if (username) {
            this.axios.get(`${api}/${username}`).then(res => {
                if (res.data) {
                    this.username = res.data.username
                    this.name = res.data.name
                    this.password = res.data.password
                }
            })
        }
/*         const key = localStorage.getItem('key')
            if(!key){
                this.$router.push('/')
            } */
    },
    methods: {
        saveData(e) {
            e.preventDefault();
            let data = {
                name: this.name,
                username: this.username,
                password: this.password
            }
            if (data) {
                this.axios.put(`${api}`, data).then(res => {
                    if (res.status==200) {
                        this.$router.push('/user')
                    }
                })
            }
        }
    }
}
</script>