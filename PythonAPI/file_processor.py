import pandas as pd
import numpy as np
from constants import *


def color_setter(number: float) -> int:
    if number > 0.75:
        return 3
    elif number > 0.5:
        return 2
    else:
        return 1


def processFile(data: pd.DataFrame) -> dict[str, pd.DataFrame]:
    data_filtered = data.drop(columns=[NEAT_COLUMN_NAME])

    blank_row_slice = data_filtered.loc[data_filtered[LABELLED_COLUMN_NAME] == BLANK_ROW_NAME]
    blank_row = [float(i) for i in blank_row_slice.loc[0, :].values.tolist()[1:]]

    data_filtered = data_filtered.loc[data_filtered[LABELLED_COLUMN_NAME] != BLANK_ROW_NAME]
    data_filtered = data_filtered.set_index(LABELLED_COLUMN_NAME)

    for column_name in data_filtered.columns.values.tolist():
        data_filtered[column_name] = data_filtered[column_name].apply(np.float)
    data_index = data_filtered.copy(deep=True)

    data_index = data_index.sub(blank_row, axis='columns')

    for column_name in data_index.columns.values.tolist():
        data_index[column_name] = data_index[column_name].abs()

    data_index = data_index.div(blank_row, axis='columns')

    data_color = data_index.copy(deep=True)
    for column_name in data_color.columns.values.tolist():
        data_color[column_name] = data_color[column_name].apply(color_setter)

    data_to_cluster = data_color[~(data_color == 1).all(axis=1)]
    wrong_data = data_color[(data_color == 1).all(axis=1)]

    return {
        DICTIONARY_INITIAL: data_filtered,
        DICTIONARY_INDEX: data_index,
        DICTIONARY_COLOR: data_to_cluster,
        DICTIONARY_WRONG: wrong_data
    }
