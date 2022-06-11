import codecs
import csv
from fastapi import FastAPI, UploadFile, Response
import pandas as pd
from cluster_service import clustering
from constants import CLUSTER_COLUMN_NAME, DICTIONARY_CLUSTERS, DICTIONARY_INITIAL, DICTIONARY_INDEX, DICTIONARY_WRONG, \
    DICTIONARY_COLOR
from file_processor import processFile

app = FastAPI()


@app.get("/")
async def root():
    return {"message": "Hello World"}


@app.post("/clustering")
async def api_clustering(file: UploadFile):
    data_color = read_data_from_file(file)
    data = processFile(data_color)
    clustered_data = clustering(data[DICTIONARY_COLOR].copy(deep=True))
    response = prepare_response(data, clustered_data)
    return response


def read_data_from_file(file: UploadFile) -> pd.DataFrame:
    data = file.file
    data = csv.reader(codecs.iterdecode(data, 'utf-8'), delimiter=',')
    header = data.__next__()
    return pd.DataFrame(data, columns=header)


def prepare_response(data: dict[str, pd.DataFrame], clustered: pd.DataFrame) -> dict[str, dict]:
    data_filtered = data[DICTIONARY_INITIAL].to_dict(orient="index")
    data_index = data[DICTIONARY_INDEX].to_dict(orient="index")
    data_wrong = data[DICTIONARY_WRONG].to_dict(orient="index")

    clusters = clustered.iloc[:, 0].to_dict()

    data_color = clustered.drop(CLUSTER_COLUMN_NAME, axis=1).to_dict(orient="index")

    response = {
        DICTIONARY_CLUSTERS: clusters,
        DICTIONARY_INITIAL: data_filtered,
        DICTIONARY_INDEX: data_index,
        DICTIONARY_COLOR: data_color,
        DICTIONARY_WRONG: data_wrong
    }

    return response
