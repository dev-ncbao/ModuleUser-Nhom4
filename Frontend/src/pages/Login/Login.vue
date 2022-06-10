<template src="./Login.html"></template>
<style src="./Login.scss" lang="scss"></style>

<script setup lang="ts">
    fetch('http://localhost:5555/users', {
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
        method: 'GET',
        credentials: 'include',
    }).then((res) => console.log(res))
    //.then((data) => console.log(data))
</script>
