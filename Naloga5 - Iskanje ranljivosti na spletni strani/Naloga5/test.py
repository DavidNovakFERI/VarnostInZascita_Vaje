import unittest
import app

class FlaskTestCase(unittest.TestCase):

    def setUp(self):
        # Set up the application for testing
        app.app.config['TESTING'] = True
        self.app = app.app.test_client()

    def test_register_user(self):
        # Test user registration
        response = self.app.post('/register', data=dict(username="newuser", password="password"), follow_redirects=True)
        self.assertEqual(response.status_code, 200)

    def test_login(self):
        # Test user login
        response = self.app.post('/login', data=dict(username="newuser", password="password"), follow_redirects=True)
        self.assertEqual(response.status_code, 200)

    def test_sql_injection(self):
        # Test SQL injection vulnerability
        response = self.app.post('/vulnerable/search', data=dict(username="admin'--"), follow_redirects=True)
        self.assertIn(b'admin', response.data)  # Check if 'admin' user details are leaked

if __name__ == '__main__':
    unittest.main()
