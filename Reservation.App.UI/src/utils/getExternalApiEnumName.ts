import { ExternalAPIsEnum } from '@/@types/api/importData'

export default function getExternalApiEnumName(
    externalApiEnum: ExternalAPIsEnum
) {
    switch (externalApiEnum) {
        case ExternalAPIsEnum.xxx:
            return 'xxx'
    }

    return ''
}
