Getting started (This will be replaced by a fabffile)

To run locally, do the usual:

1. Activate a virtualenv: workon summersauce
(consider installing a virtualenv wrapper. It will make your life easier)

2. Pull the github repo

2. Install dependencies: pip install -r requirements.txt

3 Update your schema: python manage.py migrate

4 run the ssl server: python manage.py runserver_plus --cert cert

5 Add the cert.key and cert.crt files to your .gitignore


Latest changes

1. We got rid of django-sslserver. Since we have been using django-extensions runserver_plus, which comes with a built-in ssl server, it just made sense to remove it.






