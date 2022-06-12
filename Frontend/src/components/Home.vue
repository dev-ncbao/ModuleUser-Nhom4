<template>
    <div class="login-user">
        <div class="container">
            <div class="row">
                <div class="col-sm-9 col-md-7 col-lg-5 mx-auto">
                    <div class="card border-0 shadow rounded-3 my-5">
                        <div class="card-body p-4 p-sm-5">
                            <h5 class="card-title text-center mb-5 fw-light fs-5">Sign In</h5>
                            <div>
                                <div class="form-floating mb-3">
                                    <input v-model="username" type="text" class="form-control" id="floatingInput">
                                    <label for="floatingInput">Username</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="password" type="password" class="form-control"
                                        id="floatingPassword">
                                    <label for="floatingPassword">Password</label>
                                </div>

                                <div class="form-check mb-3">
                                    <input class="form-check-input" type="checkbox" value="" id="rememberPasswordCheck"
                                        checked>
                                    <label class="form-check-label" for="rememberPasswordCheck">
                                        Nhập sai ban nick :))
                                    </label>
                                </div>
                                <div class="d-grid">
                                    <button v-on:click="loginUser"
                                        class="btn btn-primary btn-login text-uppercase fw-bold">Sign
                                        in</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { api, api2 } from '../api';
import moment from 'moment';
export default {
    data() {
        return {
            username: "",
            password: ""
        }
    },
    methods: {
        loginUser(e) {
            e.preventDefault();
            let { username, password } = this
            let data = {username,password}
            if (data) {
                this.axios.post(api2, data).then(res => {
                    if (res.status == 200) {
                        localStorage.setItem('username',data.username)
                        this.$router.push('/user')
                        // console.log(localStorage.getItem('key',this.username))
                    }
                })
                    .catch(error => {
                        alert("Ban da bi ban nick!!!")
                    })
            }
        }
    }
}
</script>

<style>
.login-user {
    background: #007bff;
    background: linear-gradient(to right, #0062E6, #33AEFF);
    width: 100vw;
    height: 100vh;
}

.btn-login {
    font-size: 0.9rem;
    letter-spacing: 0.05rem;
    padding: 0.75rem 1rem;
}
</style>