<script>
    import Button, {Label} from "@smui/button";
    import Textfield from "@smui/textfield";
    import {signIn} from "../api/authentication.js";

    export let redirect;
    let username = "";
    let password = "";

    function signInAndRedirect() {
        signIn(username, password).then(x => {
            if (typeof redirect === "string" && redirect.length > 0) {
                return window.location.href = redirect;
            }   

            return window.location.href = "/";
        }).catch(x => {
            console.error(x);
        })
    }
</script>

<div>
    <div>
        <Textfield label="Username" bind:value={username} />
    </div>
    <div>
        <Textfield label="Password" bind:value={password} type="password" />
    </div>

    <div class="mt-3">
        <Button on:click={signInAndRedirect}>
            <Label>Login</Label>
        </Button>
    </div>
</div>