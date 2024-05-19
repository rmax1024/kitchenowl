## Contributing

Take a look at the general contribution rules [here](../CONTRIBUTING.md).

### Requirements
- Python 3.11+

### Setup & Install
- If you haven't already, switch to the backend folder `cd backend`
- Create a python environment `python3 -m venv venv`
- Activate your python environment `source venv/bin/activate` (environment can be deactivated with `deactivate`)
- Install dependencies `pip3 install -r requirements.txt`
- Initialize/Upgrade the SQLite database with `flask db upgrade`
- Initialize/Upgrade requirements for the recipe scraper `python -c "import nltk; nltk.download('averaged_perceptron_tagger')"`
- Run debug server with `python3 wsgi.py` or without debugging `flask run`
- The backend should be reachable at `localhost:5000`

``` bash
   cd /mnt/c/dev/home/kitchenowl/backend/
   sudo apt install python3.11-venv
   python3.11 -m venv venv
   source venv/bin/activate
   python3.11 -m pip list
   sudo apt-get install build-essential python3.11-dev
   pip3 install -r requirements.txt
   flask db upgrade
   python3.11 wsgi.py
```
