import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from kneed import KneeLocator  # pip install kneed

from constants import CLUSTER_COLUMN_NAME, LABELLED_COLUMN_NAME

from scipy.cluster.hierarchy import dendrogram
from sklearn.cluster import AgglomerativeClustering


def plot_dendrogram(model, **kwargs):
    # Створення матриці зв'язку і побудова дендрограми

    # Обрахунок кількості точок під кожною вершиною
    counts = np.zeros(model.children_.shape[0])
    n_samples = len(model.labels_)
    for i, merge in enumerate(model.children_):
        current_count = 0
        for child_idx in merge:
            if child_idx < n_samples:
                current_count += 1  # Вершина листок
            else:
                current_count += counts[child_idx - n_samples]
        counts[i] = current_count

    linkage_matrix = np.column_stack(
        [model.children_, model.distances_, counts]).astype(float)

    # Побудова дендрограми
    dendrogram(linkage_matrix, **kwargs)
    plt.show()


def get_optimal_distance(distances, dist_method):
    # Знаходження оптимальної відстані
    K = range(0, len(distances))
    kl = KneeLocator(K, distances, curve="convex", direction="increasing", interp_method=dist_method)

    best_k = kl.elbow
    threshold = distances[best_k]

    # Побудова графіка
    ax1 = plt.axes()
    ax1.plot(distances)
    ax1.scatter(best_k, threshold, s=80, facecolors='none', edgecolors='r', linewidths=2)
    plt.show()

    print(threshold)

    return threshold


def clastering(data, method, threshold):
    if threshold is None:
        clustering = AgglomerativeClustering(linkage=method, compute_distances=True)
    else:
        clustering = AgglomerativeClustering(linkage=method, n_clusters=None, distance_threshold=threshold)
    result = clustering.fit(data)
    return (clustering, result.distances_, result.labels_)


def hierarchical(data, method, dist_method, title):
    (clustering, distances, labels) = clastering(data, method, None)
    threshold = get_optimal_distance(distances, dist_method)
    (clustering, distances, labels) = clastering(data, method, threshold)

    data_index_copy = data.copy(deep=True)
    data_index_clusterize = data_index_copy
    data_index_clusterize.insert(0, CLUSTER_COLUMN_NAME, labels, True)
    data_index_clusterize = data_index_clusterize.sort_values(by=[CLUSTER_COLUMN_NAME, LABELLED_COLUMN_NAME])

    plt.title(title, size=18)
    plot_dendrogram(clustering, labels=data_index_copy.index, truncate_mode="level",
                    p=data_index_copy.shape[0], color_threshold=threshold)

    return data_index_clusterize


def clustering(data: pd.DataFrame) -> pd.DataFrame:
    return hierarchical(data, "ward", 'polynomial', "Ward Linkage")