import matplotlib.pyplot as plt
import numpy as np
import numpy.typing as npt
import pandas as pd
from kmodes.kmodes import KModes  # pip install kmodes
from kneed import KneeLocator  # pip install kneed

from constants import cluster_column_name, labelled_column_name

# 1 to show and 0 to hide
verbose = 1


def setup_kmodes(num_clusters: int, data: pd.DataFrame) -> KModes:
    """
    Setup KModes
    :param num_clusters: amount of clusters
    :return: instance of KModes
    """
    # if num_clusters == 1:
    #     indexes = [data.shape[0] // 2]
    # else:
    #     r = list(range(0, data.shape[0]))
    #     chunks = [r[i:i + data.shape[0] // num_clusters] for i in range(0, len(r), data.shape[0] // num_clusters)]
    #     indexes = [chunks[i][len(chunks[i]) // 2] for i in range(0, len(chunks))]
    #     indexes = np.resize(indexes, num_clusters)
    #
    # centroids = data.iloc[indexes, :].values
    # return KModes(n_clusters=num_clusters, init=centroids, n_init=5, verbose=verbose)
    return KModes(n_clusters=num_clusters, init="Cao", n_init=15, verbose=verbose)


def build_elbow_curve(data: pd.DataFrame) -> tuple[range, list[int]]:
    """
    Build Elbow curve to find optimal cluster amount
    :param data: data to be clustered
    :return: range from 1 to max amount of clusters, cost of each clustered model
    """
    cost = []
    cluster_amount_range = range(1, data.shape[0])
    for num_clusters in list(cluster_amount_range):
        kmodes = setup_kmodes(num_clusters, data)
        kmodes.fit_predict(data)
        cost.append(kmodes.cost_)

    if verbose == 1:
        build_elbow_plot(cluster_amount_range, cost)
    return cluster_amount_range, cost


def build_elbow_plot(cluster_amount_range: range, cost: list[int]):
    """
    Build and show Elbow curve plot
    :param cluster_amount_range: range from 1 to max amount of clusters
    :param cost: cost of clustering
    """
    plt.plot(cluster_amount_range, cost, 'o-', color="blue", markerfacecolor='red', markeredgecolor='red')
    plt.xlabel('No. of clusters')
    plt.ylabel('Cost')
    plt.title('Elbow Method For Optimal k')
    plt.show()


def get_optimal_cluster_amount(data: pd.DataFrame) -> int:
    """
    Get exact cluster amount
    :param data: data to be clustered
    :return: optimal cluster amount base on elbow method
    """
    cluster_amount_range, cost = build_elbow_curve(data)
    kl = KneeLocator(cluster_amount_range, cost, curve="convex", direction="decreasing")
    exact_cluster_amount = kl.elbow
    return exact_cluster_amount


def get_clusters_for_optimal_model(data: pd.DataFrame, cluster_amount: int) -> npt.NDArray[np.uint16]:
    """
    Build optimal model with 'cluster_amount' clusters
    :param cluster_amount: cluster amount
    :param data: data to be clustered
    :return: list of cluster number for each data row
    """
    kmodes = setup_kmodes(cluster_amount, data)
    clusters = kmodes.fit_predict(data)
    return clusters


def format_response(data: pd.DataFrame, clusters: npt.NDArray[np.uint16]) -> pd.DataFrame:
    """
    Format response data
    :param data: data to be clustered
    :param clusters: list of cluster number for each data row
    :return: json with clustered data
    """
    data.insert(0, cluster_column_name, clusters, True)
    data = data.reset_index()
    data = data.sort_values(by=[cluster_column_name, labelled_column_name])
    data = data.set_index(labelled_column_name)
    return data


def clustering(data: pd.DataFrame) -> pd.DataFrame:
    """
    Clustering data
    :param data: data to be clustered
    :return: json with clustered data
    """
    cluster_amount = get_optimal_cluster_amount(data)
    print(f"Optimal cluster amount {cluster_amount}")
    clusters = get_clusters_for_optimal_model(data, cluster_amount)
    response = format_response(data, clusters)
    return response
