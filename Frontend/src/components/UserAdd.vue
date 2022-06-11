<template>
    <div class="container mt-4">
        <h1>Add User</h1>
        <form>
            <div class="mb-3">
                <label for="n" class="form-label">Name</label>
                <input v-model="name" type="email" class="form-control" id="n">
            </div>
            <div class="mb-3">
                <label for="un" class="form-label">User name</label>
                <input v-model="username" type="email" class="form-control" id="un">
            </div>
            <div class="mb-3">
                <label for="pw" class="form-label">Password</label>
                <input v-model="password" type="password" class="form-control" id="pw">
            </div>
            <button type="submit" class="btn btn-primary" v-on:click="addData">Add user</button>
        </form>
    </div>
</template>

<script>
import { api } from '../api';
export default {
    data() {
        return {
            username: "",
            password: "",
            name: ""
        }
    },
    mounted(){
        const key = localStorage.getItem('key')
            if(!key){
                this.$router.push('/')
            }
    },
    methods: {
        addData(e) {
            e.preventDefault();
            let { name, username, password } = this;
            let data = { name, username, password };
            if (data) {
                this.axios.post(api, data).then(res => {
                    if (res.data) {
                        this.$router.push('/user');
                    }
                })
            }
        }
    }
}
</script>

<style>
</style>