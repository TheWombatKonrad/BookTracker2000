Vue.createApp({
    data() {
        return {
            username: '',
            password: '',
            email: '',
            loggedIn: false,
            userBooks: [], //stores books the user has read/is reading
            currentlyReading: [],
            readings: [], //stores reading sessions
            library: [], //should store unread books... :/
            authors: [],
            title: '',
            pages: '',
            firstName: '',
            lastName: ''

        }
    },
    methods: {
        //login function, enter username/password, returns jwt
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

            const resJson = await response.json();

            if (response.ok) {
                localStorage.setItem("jwt", resJson.token);
                this.loggedIn = true;
                this.fetchCurrentUserData();
            }

            else {
                alert(resJson.message);
            }
        },

        logOut() {
            localStorage.clear();
        },

        //enter username, password, email
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

            const resJson = await response.json();

            if (response.ok) {
                document.getElementById("message").textContent = resJson.message
                setTimeout(() => function () {
                    window.location.href = 'index.html';
                }, 3000);

            }

            else {
                document.getElementById("message").textContent = resJson.message;
            }

            //this is emptied bc the user hasn't actually logged in yet
            this.username = '';
            this.password = '';
            this.email = '';

        },

        //sets up userBooks, username, email, calls fetchCurrentUserReadings
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

                //setting up books
                this.userBooks = resJson.bookList;
                this.currentlyReading = [];

                for (const book of this.userBooks)
                {
                    if (book.finished !== true)
                        this.currentlyReading.push(book);
                }

                //setting up user data
                this.username = resJson.username;
                this.email = resJson.email;
                this.fetchCurrentUserReadings();
            }

            else {
                confirm("There's an issue loading the data, please log out and try again.");

                if (confirm) this.logOut();
            }
        },

        //sets up currentReadings
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
                confirm("There's an issue loading the data, please log out and try again.");

                if (confirm) this.logOut();
            }
        },

        //sets up library, which should be filtered for unread books, but doesn't work correctly
        async fetchbooks() {
            const response = await fetch("https://localhost:4000/api/books", {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "GET",
            });


            if (response.ok) {
                const allBooks = await response.json();

                //to filter the books already read/reading from the library
                //NOT WORKING
                for (const book of allBooks) {
                    if (!this.userBooks.includes(x => x.id === book.id)) {
                        this.library.push(book);
                    }
                }

            }

            else {
                alert("Sorry, there's an issue connecting to the API.");
            }
        },

        //sets up authors (used when adding books), authorfirstname, authorlastname
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
                alert("Sorry, there's an issue connecting to the API.");
            }
        },

        //POSTS a reading for the current user, at the moment pagesRead is always 200
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

            const resJson = await response.json();

            if (response.ok) {
                this.fetchCurrentUserData();
            }

            else {
                alert(resJson.message);
            }
            
        },

        //POSTS a book, takes title, pages, firstname, lastname 
        async registerBook() {
            let authorId = 0;
            const jwt = localStorage.getItem("jwt");

            await this.fetchAuthors();

            //if the author already exists, authorId is set to their authorId
            for (const author of this.authors) {
                if ((author.lastName === this.lastName) && (author.firstName === this.firstName)) {
                    authorId = author.authorId;
                }
            }

            //if the author doesnt already exist, it's created
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

            const resJson = await response.json();

            //displays messages in the message bar
            if (response.ok) {
                this.fetchCurrentUserData();
                document.getElementById("message").textContent = resJson.message;
            }

            else {
                document.getElementById("message").textContent = resJson.message;
            }
        },

        //POSTS an author, takes firstname, lastname 
        async registerAuthor() {
            const jwt = localStorage.getItem("jwt");

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

            const resJson = await response.json();

            if (response.ok) {
                return resJson.id;
            }

            //displays error message in message bar
            else {
                document.getElementById("message").textContent = resJson.message;
            }
        },

        //DELETES current user account, logs the user out after
        async deleteUserAccount() {
            let del = confirm("Are you sure?");//if user presses ok, continues, if cancels, stops

            if (del) {
                const jwt = localStorage.getItem("jwt");

                const response = await fetch("https://localhost:4000/api/users/current", {
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + jwt
                    },
                    method: "DELETE",
                });

                const resJson = await response.json();

                if (response.ok) {
                    alert(resJson.message);
                }

                else {
                    alert(resJson.message);
                }

                this.logOut();
                window.location.href = 'index.html';
            }//if
        }
    },//methods

    async mounted() { 
        if (localStorage.getItem("jwt") !== null) {
            this.loggedIn = true;

            await this.fetchCurrentUserData();//must be before fetchbooks
            await this.fetchbooks();
        }   
    }

}).mount("body");



