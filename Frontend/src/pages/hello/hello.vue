<template src="./hello.html"></template>
<style src="./hello.scss" lang="scss"></style>

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
