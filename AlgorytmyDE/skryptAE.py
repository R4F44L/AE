import json
from sklearn import preprocessing
import statistics
from sklearn import svm
import sys, getopt
from sklearn.neighbors import KNeighborsClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier
from sklearn.gaussian_process import GaussianProcessClassifier
from sklearn.gaussian_process.kernels import RBF
from sklearn.ensemble import AdaBoostClassifier
from sklearn.naive_bayes import GaussianNB
from sklearn.discriminant_analysis import QuadraticDiscriminantAnalysis
from sklearn.neural_network import MLPClassifier
from joblib import dump, load

alphabet = [
    "p",
    "r",
    "a",
    "c",
    " ",
    "m",
    "g",
    "i",
    "s",
    "t",
    "e",
    "k",
    "shift1",
    "shift2",
    "backspace",
    "delete",
]
dwellAlphabet = [
    ("p", "r"),
    ("r", "a"),
    ("a", "c"),
    ("c", "a"),
    ("a", " "),
    (" ", "m"),
    ("m", "a"),
    ("a", "g"),
    ("g", "i"),
    ("i", "s"),
    ("s", "t"),
    ("t", "e"),
    ("e", "r"),
    ("r", "s"),
    ("s", "k"),
    ("k", "a"),
    ("shift", "p"),
    ("shift2", "p"),
]
min_max_scaler = preprocessing.MinMaxScaler()


dataFileName = sys.argv[1] if len(sys.argv) > 1 else "data.txt"
dataDwellFileName = sys.argv[2] if len(sys.argv) > 2 else "dataDwell.txt"
mode = sys.argv[3] if len(sys.argv) > 3 else "combined"
algorythm = sys.argv[4] if len(sys.argv) > 4 else "svc"

if algorythm == "svc":
    clf = svm.SVC(probability=True)
if algorythm == "svr":
    clf = svm.SVR()
if algorythm == "linearsvr":
    clf = svm.LinearSVR()
if algorythm == "linearsvc":
    clf = svm.LinearSVC()
if algorythm == "knn":
    clf = KNeighborsClassifier(n_neighbors=2)
if algorythm == "dtree":
    clf = DecisionTreeClassifier()
if algorythm == "rtree":
    clf = RandomForestClassifier(max_depth=2, random_state=0)
if algorythm == "gauss":
    kernel = 1.0 * RBF(1.0)
    clf = GaussianProcessClassifier(kernel=kernel, random_state=0)
if algorythm == "ada":
    clf = AdaBoostClassifier(n_estimators=100, random_state=0)
if algorythm == "quad":
    clf = QuadraticDiscriminantAnalysis()
if algorythm == "gaussnb":
    clf = GaussianNB()
if algorythm == "mlp":
    clf = MLPClassifier(random_state=1, max_iter=3000)


def scaleValues(timings):
    # X_train_minmax = min_max_scaler.fit_transform(timings)
    return timings


def entryNormalisation(timingsPerKey):
    return statistics.median(timingsPerKey)


def writeToFile(data):
    print("invoked", data)
    f = open("result.txt", "w")
    f.write(str(data))
    f.close()


def pickAndSortByAlphabet(timing):
    result = []
    for entry in timing:
        entryTimings = []
        for key in alphabet:
            keyTimings = entry.get(key, None)
            median = 0
            if keyTimings is not None:
                median = entryNormalisation(keyTimings)
            entryTimings.append(median)
        result.append(entryTimings)
    return result


def dwellSort(timing):
    result = []
    for entry in timing:
        entryTimings = []
        for keyTuple in dwellAlphabet:
            fromKeyTimings = entry.get(keyTuple[0], None)
            median = 0
            if fromKeyTimings:
                toKeyTimings = fromKeyTimings.get(keyTuple[1], None)
                if toKeyTimings:
                    median = entryNormalisation(toKeyTimings)
            entryTimings.append(median)
        result.append(entryTimings)
    return result


import os

dir_path = os.path.dirname(os.path.realpath(__file__))

try:
    clfFromFile = load("filename.joblib")
    algorythmF = open("alg.txt", "r")
    algorythmData = json.loads(algorythmF.read())
    DANE_Z_ALGORYTMU = scaleValues(algorythmData)
    result = clfFromFile.predict(DANE_Z_ALGORYTMU)
    print("PressResult from file", result)

    proba = clfFromFile.predict_proba(DANE_Z_ALGORYTMU)
    writeToFile(proba[0][0])
    print(proba)

except Exception:
    f = open("data.txt", "r")
    data = json.loads(f.read())

    dwellF = open(dataDwellFileName, "r")
    dwellData = json.loads(dwellF.read())

    users = [entry["user"] for entry in data]
    # orderDwellDataByUserFromData
    orderedDwellData = []
    for user in users:
        for entry in dwellData:
            if entry["user"] == user:
                orderedDwellData.append(entry)

    # pressTimings
    trainingTimings = [entry["firstTimings"] for entry in data]
    testTimings = [entry["secondTimings"] for entry in data]
    # print(testTimings)
    algorythmF = open("alg.txt", "r")
    algorythmData = json.loads(algorythmF.read())

    medianTest = pickAndSortByAlphabet(testTimings)
    medianTraining = pickAndSortByAlphabet(trainingTimings)
    medianTrainingWithScale = scaleValues(medianTraining)
    medianTestWithScale = scaleValues(medianTest)
    print("median", algorythmData)
    DANE_Z_ALGORYTMU = scaleValues(algorythmData)
    print("dane", DANE_Z_ALGORYTMU)
    clf.fit(medianTrainingWithScale, range(0, len(medianTrainingWithScale)))
    result = clf.predict(DANE_Z_ALGORYTMU)
    print("PressResult", result)
    proba = clf.predict_proba(DANE_Z_ALGORYTMU)
    writeToFile(proba[0][0])
    print(proba)
    dump(clf, "filename.joblib")
