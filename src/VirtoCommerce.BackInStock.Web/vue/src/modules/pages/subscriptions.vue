<template>
  <VcBlade
    :title="$t('BACK_IN_STOCK.LIST.TITLE')"
    :toolbar-items="bladeToolbar"
    :loading="loading"
    width="50%"
  >
    <VcDataTable
      v-model:search-value="searchValue"
      v-model:sort-field="sortField"
      v-model:sort-order="sortOrder"
      :items="items"
      :total-count="totalCount"
      :pagination="pagination"
      :searchable="true"
      state-key="BACK_IN_STOCK_SUBSCRIPTIONS"
      :active-item-id="activeItemId"
      :empty-state="{ icon: 'lucide-bell-ring', title: $t('BACK_IN_STOCK.LIST.EMPTY') }"
      @row-click="(e: { data: BackInStockSubscription }) => openProduct(e.data)"
      @pagination-click="pagination.goToPage"
    >
      <VcColumn
        id="productName"
        :title="$t('BACK_IN_STOCK.LIST.COLUMNS.PRODUCT_NAME')"
        mobile-role="title"
        sortable
        always-visible
      />
      <VcColumn
        id="productCode"
        :title="$t('BACK_IN_STOCK.LIST.COLUMNS.PRODUCT_CODE')"
        mobile-role="field"
        sortable
      />
      <VcColumn
        id="storeId"
        :title="$t('BACK_IN_STOCK.LIST.COLUMNS.STORE')"
        mobile-role="field"
        sortable
      />
      <VcColumn
        id="isActive"
        :title="$t('BACK_IN_STOCK.LIST.COLUMNS.STATUS')"
        mobile-role="status"
        width="120px"
        sortable
        always-visible
      >
        <template #body="{ data }">
          <div class="dash-inline">
            <VcStatus :variant="data.isActive ? 'success' : 'info'">
              {{ $t(data.isActive ? "BACK_IN_STOCK.LIST.ACTIVE" : "BACK_IN_STOCK.LIST.INACTIVE") }}
            </VcStatus>
          </div>
        </template>
      </VcColumn>
    </VcDataTable>
  </VcBlade>
</template>

<script setup lang="ts">
// back-in-stock-subscription-list: subscriptions of a contact (opened from the contact blade widget); a row opens the product
import {
  notification,
  useApiClient,
  useAsync,
  useBlade,
  useDataTablePagination,
  useDataTableSort,
  useFunctions,
  useTableSearch,
  type IBladeToolbar,
} from "@vc-shell/framework";
import { VcBlade, VcColumn, VcDataTable, VcStatus } from "@vc-shell/framework/ui";
import { ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import {
  BackInStockSubscriptionClient,
  type BackInStockSubscription,
} from "../../api_client/virtocommerce.backinstock";

defineBlade({
  url: "/back-in-stock-subscriptions",
  name: "BackInStockSubscriptions",
  permissions: ["BackInStock:subscription:read"],
});

const PAGE_SIZE = 20;
const { t } = useI18n({ useScope: "global" });
// the contact (member) id is the param, so that a reload keeps the list scoped to it
const { param, openBlade } = useBlade();
const { getApiClient } = useApiClient(BackInStockSubscriptionClient);
const { debounce } = useFunctions();

const items = ref<BackInStockSubscription[]>([]);
const totalCount = ref(0);
const activeItemId = ref<string>();

const { sortField, sortOrder, sortExpression } = useDataTableSort({ stateKey: "BACK_IN_STOCK_SUBSCRIPTIONS" });
const { searchValue } = useTableSearch({ stateKey: "BACK_IN_STOCK_SUBSCRIPTIONS" });
const pagination = useDataTablePagination({ stateKey: "BACK_IN_STOCK_SUBSCRIPTIONS", pageSize: PAGE_SIZE, totalCount });

const { loading, action: load } = useAsync(async () => {
  if (!param.value) return;
  const r = await (await getApiClient()).search({
    memberId: param.value,
    keyword: searchValue.value || undefined,
    sort: sortExpression.value,
    skip: pagination.skip,
    take: PAGE_SIZE,
  });
  items.value = r.results ?? [];
  totalCount.value = r.totalCount ?? 0;
});

load();
watch(searchValue, () => pagination.setPage(1));
watch([sortExpression, searchValue, () => pagination.skip], debounce(load, 300));

async function openProduct(item: BackInStockSubscription) {
  // the Catalog plugin owns the product blade; it may not be installed in this host
  await openBlade({
    name: "CatalogProductDetails",
    param: item.productId,
    onOpen: () => (activeItemId.value = item.id),
    onClose: () => (activeItemId.value = undefined),
  }).catch(() => notification.error(t("BACK_IN_STOCK.LIST.PRODUCT_UNAVAILABLE"), { timeout: 5000 }));
}

const bladeToolbar = ref<IBladeToolbar[]>([
  { id: "refresh", title: t("BACK_IN_STOCK.LIST.REFRESH"), icon: "lucide-refresh-cw", clickHandler: () => load() },
]);
</script>
