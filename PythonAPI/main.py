import codecs
import csv
from fastapi import FastAPI, UploadFile, Response
import pandas as pd
from cluster_service import clustering
from constants import cluster_column_name
from file_processor import processFile

app = FastAPI()


@app.get("/")
async def root():
    return {"message": "Hello World"}


@app.post("/clustering")
async def api_clustering(file: UploadFile):
    data_color = read_data_from_file(file)
    data = processFile(data_color)
    clustered_data = clustering(data[2].copy(deep=True))
    response = prepare_response(data, clustered_data)
    return response


def read_data_from_file(file: UploadFile) -> pd.DataFrame:
    data = file.file
    data = csv.reader(codecs.iterdecode(data, 'utf-8'), delimiter=';')
    header = data.__next__()
    return pd.DataFrame(data, columns=header)


def prepare_response(data: tuple[pd.DataFrame, pd.DataFrame, pd.DataFrame], clustered: pd.DataFrame) -> object:
    data_filtered = data[0].to_dict(orient="index")
    data_index = data[1].to_dict(orient="index")

    clusters = clustered.iloc[:, 0].to_dict()

    data_color = clustered.drop(cluster_column_name, axis=1).to_dict(orient="index")

    response = {
        "clusters": clusters,
        "initial": data_filtered,
        "index": data_index,
        "color": data_color,
    }
    return response
