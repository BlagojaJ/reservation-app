function objectToFormData<T>(obj: T): FormData {
    const formData = new FormData()

    for (const key in obj) {
        const value = obj[key]

        if (value instanceof Array) {
            // If the value is a FileList (e.g., from an input[type=file] element),
            // append each file individually
            for (let i = 0; i < value.length; i++) {
                formData.append(key, value[i])
            }
        } else if (value instanceof File) {
            // If the value is a File, append it directly
            formData.append(key, value)
        } else {
            // Otherwise, convert the value to a string and append
            formData.append(key, value ? String(value) : '')
        }
    }

    return formData
}

export default objectToFormData
