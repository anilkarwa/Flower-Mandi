<template>
    <div id="login">
  <v-app id="inspire">
    <v-content>
      <v-container style="background-color:  #f5b041 " fluid fill-height>
        <v-layout  align-center justify-center>
          <v-flex xs12 sm8 md4 v-if="login">
            <v-card class="elevation-12">
              <v-toolbar dark color="primary">
                <v-toolbar-title> Mandi - Login </v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>
              <v-card-text>
                <v-form v-model="valid" ref="form">
                  <v-text-field
                  v-model="loginDetails.username"
                  id="username"
                  label="User Name" required @change="changeCase" ></v-text-field>
                  <v-text-field
                  v-model="loginDetails.password"
                  id="password"
                  type="password"
                  :rules="passwordRules"
                  label="Password" required ></v-text-field>
                </v-form>
              </v-card-text>
              <v-card-actions>
                <v-btn @click="changeScreen('reset')" >Reset password</v-btn>
                <v-spacer></v-spacer>
                <v-btn @click="changeScreen('register')" >Register</v-btn>
                <v-spacer></v-spacer>
                <v-btn @click="loginUser" color="primary">Login</v-btn>
              </v-card-actions>
              <v-snackbar
                :timeout="snackbarTimeout"
                :color="snackbarColor"
                :multi-line="snackbarMode === 'multi-line'"
                :vertical="snackbarMode === 'vertical'"
                v-model="snackbar" > {{ snackbarText }} <v-btn dark flat @click.native="snackbar = false">Close</v-btn>
              </v-snackbar>
            </v-card>
          </v-flex>
            <v-flex xs12 sm8 md4 v-if="forgetPassword">
            <v-card class="elevation-12">
              <v-toolbar dark color="primary">
                <v-toolbar-title> Mandi - Forgot Password </v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>
              <v-card-text>
                <v-form v-model="valid" ref="form">
                  <v-text-field
                  v-model="loginDetails.oldPassword"
                  id="Old Password"
                  type="password"
                  :rules="[v => !!v || 'Old password is required']"
                  label="Old Password" required ></v-text-field>
                  <v-text-field
                  v-model="loginDetails.newPassword"
                  id="New Password"
                  type="password"
                  :rules="[v => !!v || 'Password is required']"
                  label="New Password" required ></v-text-field>
                  <v-text-field
                  v-model="loginDetails.confirmPassword"
                  id="Confirm Password"
                  type="password"
                  :rules="[v => !!v || 'Confirm password is required']"
                  label="Confirm Password" required ></v-text-field>
                </v-form>
              </v-card-text>
              <v-card-actions>
                <v-btn @click="changeScreen('login')" >Login</v-btn>
                <v-spacer></v-spacer>
                <v-btn @click="changeScreen('register')" >Register</v-btn>
                <v-spacer></v-spacer>
                <v-btn @click="saveNewPassword" color="primary">Save</v-btn>
              </v-card-actions>
              <v-snackbar
                :timeout="snackbarTimeout"
                :color="snackbarColor"
                :multi-line="snackbarMode === 'multi-line'"
                :vertical="snackbarMode === 'vertical'"
                v-model="snackbar" > {{ snackbarText }} <v-btn dark flat @click.native="snackbar = false">Close</v-btn>
              </v-snackbar>
            </v-card>
          </v-flex>
          <v-flex xs12 sm8 md4 v-if="signup">
            <v-card class="elevation-12">
              <v-toolbar dark color="primary">
                <v-toolbar-title>Mandi - SignUp</v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>
              <v-card-text>
                <v-form v-model="signupValid" ref="signupform">
                  <v-text-field
                  v-model="signupDetails.CompanyName"
                  id="CompanyName"
                  :rules="[v => !!v || 'Name is required']"
                  label="Company Name" required ></v-text-field>
                  <v-text-field
                  v-model="signupDetails.userName"
                  id="Username"
                  :rules="[v => !!v || 'username is required']"
                  label="User Name" required  @change="changeCaseSignup"></v-text-field>
                  <v-text-field
                  v-model="signupDetails.Password"
                  id="Password" type="password"
                  :rules="[v => !!v || 'password is required']"
                  label="Password" required ></v-text-field>
                    <v-text-field
                  v-model="signupDetails.confirmPassword"
                  id="ConfirmPassword" type="password"
                  :rules="[v => !!v || 'Confirm password is required']"
                  label="Confirm Password" required ></v-text-field>
                </v-form>
              </v-card-text>
              <v-card-actions>
                <v-btn @click="changeScreen('login')" >Login</v-btn>
                <v-spacer></v-spacer>
                <v-btn @click="signupUser()" color="primary">Save</v-btn>
              </v-card-actions>
              <v-snackbar
                :timeout="snackbarTimeout"
                :color="snackbarColor"
                :multi-line="snackbarMode === 'multi-line'"
                :vertical="snackbarMode === 'vertical'"
                v-model="snackbar" > {{ snackbarText }} <v-btn dark flat @click.native="snackbar = false">Close</v-btn>
              </v-snackbar>
            </v-card>
          </v-flex>
        </v-layout>
      </v-container>
    </v-content>
  </v-app>
</div>
</template>
<script>
import router from '../router'
export default {
  data () {
    return {
      valid: false,
      login: true,
      forgetPassword: false,
      signup: false,
      signupValid: false,
      subciptionflag: false,
      signupDetails: {
        CompanyName: '',
        userName: '',
        Password: '',
        confirmPassword: ''
      },
      loginDetails: {
        username: '',
        password: '',
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
      },
      emailRules: [
        v => !!v || 'E-mail is required',
        v => /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'E-mail must be valid'
      ],
      passwordRules: [
        a => !!a || 'Password is required',
        a => a.length <= 25 || 'Password must be less than 25 characters'
      ],
      firstNameRules: [
        b => !!b || 'First Name is required'
      ],
      lastNameRules: [
        c => !!c || 'Last Name is required'
      ],
      emailIdRules: [
        d => !!d || 'E-mail is required',
        d => /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(d) || 'E-mail must be valid'
      ],
      phoneNumberRules: [
        v => !!v || 'Phone number is required',
        v => /^(0|[+91]{3})?[7-9][0-9]{9}$/.test(v) || 'Phone number must be valid'
      ],
      snackbar: false,
      snackbarColor: 'error',
      snackbarMode: 'multi-line',
      snackbarTimeout: 6000,
      snackbarText: '',
      rowCount: 0
    }
  },
  methods: {
    changeScreen (value) {
      if (value === 'reset') {
        this.forgetPassword = true
        this.login = false
        this.signup = false
      }
      if (value === 'register') {
        this.forgetPassword = false
        this.login = false
        this.signup = true
      }
      if (value === 'login') {
        this.forgetPassword = false
        this.login = true
        this.signup = false
      }
    },
    changeCase () {
      this.loginDetails.username = this.loginDetails.username.toUpperCase()
      console.log('after uppercase username=', this.loginDetails.username)
    },
    changeCaseSignup () {
      this.signupDetails.userName = this.signupDetails.userName.toUpperCase()
    },
    loginUser () {
      if (this.$refs.form.validate()) {
        const name = this.loginDetails.username
        const password = this.loginDetails.password
        if (name === localStorage.getItem('userName') && password === localStorage.getItem('userPassword')) {
          localStorage.removeItem('itemArray')
          localStorage.removeItem('orderId')
          localStorage.setItem('userLoggedIn', 'yes')
          router.push({name: 'HelloWorld', params: {resource: 'Test'}})
        } else {
          this.snackbar = true
          this.snackbarColor = 'error'
          this.snackbarText = 'Invalid credenticals, Try again!'
        }
      } else {
        this.$refs.form.validate()
      }
    },
    signupUser () {
      console.log('Company -- sign')
      if (this.$refs.signupform.validate()) {
        console.log('Company', this.signupDetails.CompanyName)
        const companyName = this.signupDetails.CompanyName
        const usernName = this.signupDetails.userName
        const password = this.signupDetails.Password
        const confirmPassword = this.signupDetails.confirmPassword
        if (password !== confirmPassword) {
          this.snackbar = true
          this.snackbarText = 'Password dont match, Try again!'
          this.signupDetails.Password = ''
          this.signupDetails.confirmPassword = ''
        } else {
          localStorage.setItem('companyName', companyName)
          localStorage.setItem('userName', usernName)
          localStorage.setItem('userPassword', password)
          this.snackbar = true
          this.snackbarColor = 'success'
          this.snackbarText = 'Saved User, Please Login'
          this.signup = false
          this.login = true
        }
      } else {
        this.$refs.signupform.validate()
      }
    },
    saveNewPassword () {
      const oldPassword = this.loginDetails.oldPassword
      const newPassword = this.loginDetails.newPassword
      const confirmPassword = this.loginDetails.confirmPassword
      if (oldPassword !== localStorage.getItem('userPassword')) {
        this.snackbar = true
        this.snackbarText = 'Invalid old password, Try again!'
        this.loginDetails.oldPassword = ''
      } else {
        localStorage.setItem('userPassword', newPassword)
        this.snackbar = true
        this.snackbarColor = 'success'
        this.snackbarText = 'Password Changes Sucessfully'
        this.login = true
        this.forgetPassword = false
      }
      if (newPassword !== confirmPassword) {
        this.snackbar = true
        this.snackbarColor = 'error'
        this.snackbarText = 'Password dont match, Try again!'
        this.loginDetails.newPassword = ''
        this.loginDetails.confirmPassword = ''
      }
    }
  }
}
</script>
<style>

</style>
