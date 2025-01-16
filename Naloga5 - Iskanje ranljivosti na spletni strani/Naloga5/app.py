from flask import Flask, request, render_template, redirect, url_for, session
from flask_sqlalchemy import SQLAlchemy
from werkzeug.security import generate_password_hash, check_password_hash
from flask_login import LoginManager, UserMixin, login_user, login_required, logout_user, current_user
from flask_wtf import CSRFProtect
from flask_wtf.csrf import generate_csrf

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///database.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['SECRET_KEY'] = 'replace_with_a_real_secret_key'

db = SQLAlchemy(app)
csrf = CSRFProtect(app)

login_manager = LoginManager()
login_manager.init_app(app)

class User(db.Model, UserMixin):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    password_hash = db.Column(db.String(128))
    role = db.Column(db.String(20), default='user')  # default role is 'user'

    def set_password(self, password):
        self.password_hash = generate_password_hash(password)

    def check_password(self, password):
        return check_password_hash(self.password_hash, password)

@login_manager.user_loader
def load_user(user_id):
    return User.query.get(int(user_id))

@app.route('/')
def index():
    # If you want to show user details on the index page, ensure the user is logged in
    if not current_user.is_authenticated:
        return redirect(url_for('login'))
    return render_template('index.html', user=current_user)

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        user = User.query.filter_by(username=username).first()
        if user and user.check_password(password):
            login_user(user)
            return redirect(url_for('index'))
        else:
            return 'Failed to log in', 401
    csrf_token = generate_csrf()
    session['csrf_token'] = csrf_token
    return render_template('login.html', csrf_token=csrf_token)

@app.route('/register', methods=['GET', 'POST'])
def register():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        existing_user = User.query.filter_by(username=username).first()
        if existing_user is None:
            new_user = User(username=username)
            new_user.set_password(password)
            db.session.add(new_user)
            db.session.commit()
            login_user(new_user)
            return redirect(url_for('index'))
        return 'This username is already taken', 409
    csrf_token = generate_csrf()
    session['csrf_token'] = csrf_token
    return render_template('register.html', csrf_token=csrf_token)

# A03 Vulnerability
@app.route('/vulnerable/search', methods=['POST'])
def vulnerable_search():
    username = request.form['username']
    raw_sql = f"SELECT * FROM user WHERE username = '{username}'"
    result = db.engine.execute(raw_sql)
    users = result.fetchall()
    return render_template('index.html', users=users)

# A01 Vulnerability (Intentionally Broken Access Control)
@app.route('/profile/<username>')
@login_required
def view_profile(username):
    user = User.query.filter_by(username=username).first()
    if not user:
        return "User not found", 404
    # Here user is correctly being passed to the template
    return render_template('profile.html', user=user)

if __name__ == '__main__':
    with app.app_context():
        db.create_all()
    app.run(debug=True)
