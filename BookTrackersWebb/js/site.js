Vue.createApp({
    data() {
        return {
            username: '',
            password: '',
            email: '',
            loggedIn: false,
            userBooks: [],
            currentlyReading: [],
            readings: [],
            library: [],
            authors: [],
            title: '',
            pages: '',
            firstName: '',
            lastName: ''

        }
    },
    methods: {
        async login() {
            const response = await fetch("https://localhost:4000/api/users/authenticate", {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "POST",
                body: JSON.stringify({
                    "Username": this.username,
                    "Password": this.password
                })
            });
              

            if (response.ok) {
                const resJson = await response.json();
                localStorage.setItem("jwt", resJson.token);
                localStorage.username = this.username; 
                //set frontend stuff
            }

            else {
                //send error
            }

            this.loggedIn = true;
        },

        logout() {
            localStorage.clear();
        },


        async registerUser() {
            const response = await fetch("https://localhost:4000/api/users/register", {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "POST",
                body: JSON.stringify({
                    "Username": this.username,
                    "Password": this.password,
                    "Email": this.email
                })
            });

            if (response.ok) {
                document.getElementById("message").textContent = response.statusText;
                setTimeout(() => function () {
                    window.location.href = 'index.html';
                }, 3000);

            }

            else {
                document.getElementById("message").innerHTML = response.statusText;
            }

            this.username = '';
            this.password = '';
            this.email = '';

            window.location.href = 'index.html';
        },

        async fetchCurrentUserData() {
            const jwt = localStorage.getItem("jwt");

            const response = await fetch("https://localhost:4000/api/users/current", {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + jwt
                },
                method: "GET",
            });


            if (response.ok) {
                const resJson = await response.json()
                this.userBooks = resJson.bookList;

                this.currentlyReading = [];

                for (const book of this.userBooks)
                {
                    if (book.finished !== true)
                        this.currentlyReading.push(book);
                    console.log(this.currentlyReading);
                }
            }

            else {
                //send error
            }

            this.fetchCurrentUserReadings();
        },

        async fetchCurrentUserReadings() {
            const jwt = localStorage.getItem("jwt");

            const response = await fetch("https://localhost:4000/api/readings/current", {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + jwt
                },
                method: "GET",
            });

            if (response.ok) {
                this.readings = await response.json();
            }

            else {
                //send error
            }
        },

        async fetchbooks() {
            const response = await fetch("https://localhost:4000/api/books", {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "GET",
            });


            if (response.ok) {
                this.library = await response.json()
            }

            else {
                //send error
            }
        },

        async fetchAuthors() {
            const response = await fetch("https://localhost:4000/api/authors", {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "GET",
            });


            if (response.ok) {
                this.authors = await response.json()
            }

            else {
                //send error
            }
        },

        async registerReading(bookId) {
            const jwt = localStorage.getItem("jwt");

            const response = await fetch("https://localhost:4000/api/readings", {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + jwt
                },
                body: JSON.stringify({
                    "BookId": bookId,
                    "PagesRead": 200
                }),
                method: "POST",
            });

            this.fetchCurrentUserData();
        },

        async registerBook() {
            let authorId = 0;
            const jwt = localStorage.getItem("jwt");

            this.fetchAuthors();

            for (const author of this.authors) {
                if ((author.lastName === this.lastName) && (author.firstName === this.firstName)) {
                    authorId = author.authorId;
                }
            }

            if (authorId === 0) {
                authorId = await this.registerAuthor();
            }

            const response = await fetch("https://localhost:4000/api/books", {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + jwt
                },
                body: JSON.stringify({
                    "Title": this.title,
                    "AuthorId": authorId,
                    "Pages": this.pages
                }),
                method: "POST",
            });

            if (response.ok) {
                //message book added
            }

            else {
                //send error
            }

            window.location.href = 'index.html';

        },

        async registerAuthor() {
            const jwt = localStorage.getItem("jwt");
            let authorId = 0;

            const response = await fetch("https://localhost:4000/api/authors", {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + jwt
                },
                body: JSON.stringify({
                    "FirstName": this.firstName,
                    "LastName": this.lastName
                }),
                method: "POST",
            });

            if (response.ok) {
                const resJson = await response.json()
                return resJson.id;
            }

            else {
                //send error
            }

        }
    },//methods

    mounted() { 
        if (localStorage.getItem("jwt") !== null) {
            this.loggedIn = true;
            this.username = localStorage.username;

            this.fetchCurrentUserData();
            this.fetchbooks();
        }   
    }

}).mount("body");



